using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using InventoryManagementAPI.Data;
using InventoryManagementAPI.Dto;
using InventoryManagementAPI.Helpers;
using InventoryManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementAPI.Controllers
{
    [Route("api/inventories")]
    public class InventoriesController : Controller
    {

        private readonly IInventoryRepository _invRepo;
        private readonly IProductRepository _productRepo;
        private readonly ILocationRepository _locationRepo;
        private readonly ITransactionRepository _transRepo;
        private readonly IMapper _mapper;


        public InventoriesController(IInventoryRepository invRepo, IMapper mapper, 
            IProductRepository productRepo, 
            ILocationRepository locationRepo, ITransactionRepository transRepo )
        {
            _invRepo = invRepo;
            _productRepo = productRepo;
            _locationRepo = locationRepo;
            _transRepo = transRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetInventories([FromQuery] InventoryParams inventoryParams)
        {
            var inventories = await _invRepo.GetInventories(inventoryParams);

            var inventoriesToReturn = _mapper.Map<ICollection<InventoryListDto>>(inventories);

            Response.AddPagination(inventories.CurrentPage, inventories.PageSize, 
                inventories.TotalCount, inventories.TotalPages);

            return Ok(inventoriesToReturn);


        }

        [HttpGet("{id}", Name ="GetInventory")]
        public async Task<IActionResult> GetInventory(int id)
        {
            var inventory = await _invRepo.GetInventory(id);

            if (inventory == null)
                return NotFound();

            var invetoryToReturn = _mapper.Map<InventoryDetailDto>(inventory);

            return Ok(invetoryToReturn);
        }

        [HttpGet("{id}/transactions")]
        public async Task<IActionResult> GetTransactionsByInventoryId(int id, [FromQuery] TransactionParams transParams)
        {
            var transactions = await _transRepo.GetTransactionsByInventoryId(id, transParams);

            var transactionsToReturn = _mapper.Map<ICollection<TransactionListDto>>(transactions);

            Response.AddPagination(transactions.CurrentPage, 
                transactions.PageSize, transactions.TotalCount, transactions.TotalPages);

            return Ok(transactionsToReturn);


        }

        [HttpPost("{id}/transactions")]
        public async Task<IActionResult> CreateTransactions(int id, [FromBody] TransactionCreateDto newTransaction)
        {
            var inventory = await _invRepo.GetInventory(id);

            if (inventory == null)
                return NotFound(new { error = new string[] { "Inventory not Found" } });

            var transactionType = await _transRepo.GetTransactionType(newTransaction.Transaction);

            if (transactionType == null)
                return NotFound(new { error = new string[] { "Transaction Type not Found" } });

            if(newTransaction.Quantity <= 0)
                ModelState.AddModelError("error", "Quantity Should be greater than Zero");


            if (string.Equals(transactionType.Action, "add")){

                inventory.Quantity += newTransaction.Quantity;

                if(inventory.Quantity > inventory.ThresholdCritical && inventory.Quantity <= inventory.ThresholdWarning)
                {
                    var status = await _invRepo.GetInventoryStatusByName("Warning");
                    inventory.Status = status;

                }else if(inventory.Quantity > 0 && inventory.Quantity <= inventory.ThresholdCritical)
                {
                    var status = await _invRepo.GetInventoryStatusByName("Critical");
                    inventory.Status = status;

                }else if(inventory.Quantity > inventory.ThresholdWarning)
                {
                    var status = await _invRepo.GetInventoryStatusByName("OK");
                    inventory.Status = status;
                }
            }

            if(string.Equals(transactionType.Action, "subtract"))
            {
                var quantity = inventory.Quantity - newTransaction.Quantity;

                if(quantity >= 0)
                {
                    inventory.Quantity = quantity;

                    if (inventory.Quantity > inventory.ThresholdCritical && inventory.Quantity <= inventory.ThresholdWarning)
                    {
                        var status = await _invRepo.GetInventoryStatusByName("Warning");
                        inventory.Status = status;

                    }
                    else if (inventory.Quantity > 0 && inventory.Quantity <= inventory.ThresholdCritical)
                    {
                        var status = await _invRepo.GetInventoryStatusByName("Critical");
                        inventory.Status = status;

                    }else if(inventory.Quantity == 0)
                    {
                        var status = await _invRepo.GetInventoryStatusByName("No Stock");
                        inventory.Status = status;
                    }
                }
                else
                {
                    ModelState.AddModelError("error", "Must not make quantity Negative");
                }
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var transaction = new InventoryTransaction
            {
                TransactionType = transactionType,
                Quantity = newTransaction.Quantity,
                Details = newTransaction.Details,
                TimeStamp = DateTime.Now,
                Inventory = inventory
            };

            _transRepo.Add(transaction);

            if (await _transRepo.Save())
            {
                var transactionToReturn = _mapper.Map<TransactionCreatedDto>(transaction);

                return Ok(transactionToReturn);

            }else
            {
                return BadRequest(new { error = new string[] { "Error saving transaction" } });
            }
                
        }


        [HttpPut("{inventoryId}/transactions/{transactionId}")]
        public async Task<IActionResult> UpdateTransactions(int inventoryId, int transactionId, 
            [FromBody] TransactionUpdateDto updateTransaction)
        {
            var inventory = await _invRepo.GetInventory(inventoryId);

            if (inventory == null)
                return NotFound(new { error = new string[] { "Inventory Not Found" } });

            var transaction = await _transRepo.GetInventoryTransaction(transactionId);

            if(transaction == null)
                return NotFound(new { error = new string[] { "Transaction Not Found" } });

            var transactionType = await _transRepo.GetTransactionType(updateTransaction.Transaction);

            if (transactionType == null)
                return NotFound(new { error = new string[] { "Transaction Type not Found" } });

            if (updateTransaction.Quantity < 0)
                ModelState.AddModelError("error", "quantity must be a positive number");

            //Revert Back the Inventory Count prior to the Transaction.
            //If Add, then subtract the transaction original quantity to the Inventory Quantity
            //If Subtract, then add the transaction original quantity to the Inventory Quantity
            if (string.Equals(transaction.TransactionType.Action, "add"))
                inventory.Quantity -= transaction.Quantity;

            if (string.Equals(transaction.TransactionType.Action, "subtract"))
                inventory.Quantity += transaction.Quantity;

            //Check what is the updated TransactionType.
            //After reverting the inventory quantity prior to transaction.
            //Update the inventory count according to the Action and Quantity of the updatedtransaction
            if (string.Equals(transactionType.Action, "add"))
                inventory.Quantity += updateTransaction.Quantity;
    

            if(string.Equals(transactionType.Action, "subtract"))
            {
                //For Subtract need to make sure that the updatedTransaction wont make the quantity negative.
                var quantity = inventory.Quantity -= transaction.Quantity;

                if(quantity < 0)
                {
                    ModelState.AddModelError("error", "Inventory quantity cannot be negative");
                }else
                {
                    inventory.Quantity = quantity;
                }

            }

            //Check and adjust the status based on the Threshold and Critical
            if (inventory.Quantity > inventory.ThresholdCritical && inventory.Quantity <= inventory.ThresholdWarning)
            {
                var status = await _invRepo.GetInventoryStatusByName("Warning");
                inventory.Status = status;

            }
            else if (inventory.Quantity > 0 && inventory.Quantity <= inventory.ThresholdCritical)
            {
                var status = await _invRepo.GetInventoryStatusByName("Critical");
                inventory.Status = status;

            }
            else if (inventory.Quantity > inventory.ThresholdWarning)
            {
                var status = await _invRepo.GetInventoryStatusByName("OK");
                inventory.Status = status;
            }


            //Check for ModelState Error
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            //Update the Transaction based on the updatedTransaction sent
            transaction.Quantity = updateTransaction.Quantity;
            transaction.TransactionType = transactionType;
            transaction.Details = updateTransaction.Details;
            transaction.TimeStamp = DateTime.Now;


            if (await _transRepo.Save())
            {
                var transactionToReturn = _mapper.Map<TransactionUpdatedDto>(transaction);

                return Ok(transactionToReturn);

            }
            else
            {
                return BadRequest(new { error = new string[] { "Error saving transaction" } });
            }
        }



        [HttpGet("statuses")]
        public async Task<IActionResult> GetInventoryStatuses()
        {
            var statuses = await _invRepo.GetInventoryStatuses();

            var statusesToReturn = _mapper.Map<ICollection<InventoryStatusListDto>>(statuses);

            return Ok(statusesToReturn);
        }


        [HttpPost]
        public async Task<IActionResult> CreateInventory([FromBody] InventoryCreateDto inventoryCreate)
        {
            if (inventoryCreate == null)
                return BadRequest(new { error = new string[] { "Invalid Form Data sent to the server " }});

            if (string.IsNullOrEmpty(inventoryCreate.Product))
                return BadRequest(new { error = "Product Name is Required" });

            if (string.IsNullOrEmpty(inventoryCreate.Location))
                return BadRequest(new { error = "Location Name is Required" });

            
            var product = await _productRepo.GetProductByName(inventoryCreate.Product);

            if (product == null)
                ModelState.AddModelError("error", "Product Not Found");

            var location = await _locationRepo.GetLocationByName(inventoryCreate.Location);

            if (location == null)
                ModelState.AddModelError("error", "Location not Found");

            var status = await _invRepo.GetInventoryStatusByName("No Stock");

            if (status == null)
                ModelState.AddModelError("error", "Status Not Found");

            if (string.IsNullOrEmpty(inventoryCreate.Sku))
            {
                ModelState.AddModelError("error", "SKU is Required");
            }
            else
            {
                //Check if SKU is existing
                var inventoryBySku = await _invRepo.GetInventoryBySku(inventoryCreate.Sku);

                //IF SKU is not existing - update the sku. Else return Error that SKU already exist.
                //This means that the combination of Product and Location already exist.
                if (inventoryBySku != null)
                {
                    ModelState.AddModelError("error", "SKU Already Exist");
                }
            }

            if (inventoryCreate.ThresholdCritical >= inventoryCreate.ThresholdWarning)
                ModelState.AddModelError("error", "Critical should be less than Warning");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            Inventory newIventory = new Inventory
            {
                Sku = inventoryCreate.Sku,
                Quantity = 0,
                ThresholdWarning = inventoryCreate.ThresholdWarning,
                ThresholdCritical = inventoryCreate.ThresholdCritical,
                Product = product,
                Location = location,
                Status = status
            };

            _invRepo.Add(newIventory);

            if (await _invRepo.Save())
            {
                var invetoryToReturn = _mapper.Map<InventoryDetailDto>(newIventory);

                return CreatedAtRoute(new { controller = "inventories", id = newIventory.Id }, invetoryToReturn);

            }

            return BadRequest(new { error = "Error creating inventory" });


        }

       

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInventory(int id, [FromBody] InventoryUpdateDto inventoryUpdate)
        {
            var inventory = await _invRepo.GetInventory(id);

            if (inventory == null)
                return NotFound();

            if (inventory.Sku != inventoryUpdate.Sku)
            {
                //Check if SKU is existing
                var inventoryBySku = await _invRepo.GetInventoryBySku(inventoryUpdate.Sku);

                //IF SKU is not existing - update the sku. Else return Error that SKU already exist.
                //This means that the combination of Product and Location already exist.
                if (inventoryBySku == null)
                {
                    inventory.Sku = inventoryUpdate.Sku;
                }
                else
                {
                    ModelState.AddModelError("error", "SKU Already Exist");
                }

            }


            if (inventory.Product.Name != inventoryUpdate.Product)
            {
                var product = await _productRepo.GetProductByName(inventoryUpdate.Product);

                if (product == null)
                {
                    ModelState.AddModelError("error", "Product Does not Exist");
                }
                else
                {
                    inventory.Product = product;
                }

            }

            if (inventory.Location.Name != inventoryUpdate.Location)
            {
                var location = await _locationRepo.GetLocationByName(inventoryUpdate.Location);

                if (location == null)
                {
                    ModelState.AddModelError("error", "Location Does not Exist");

                }
                else
                {
                    inventory.Location = location;
                }


            }

            if (inventoryUpdate.ThresholdCritical >= inventoryUpdate.ThresholdWarning)
            {
                ModelState.AddModelError("error", "Critical should be less than the Warning");

            }
            else
            {
                if (inventory.ThresholdCritical != inventoryUpdate.ThresholdCritical)
                    inventory.ThresholdCritical = inventoryUpdate.ThresholdCritical;

                if (inventory.ThresholdWarning != inventoryUpdate.ThresholdWarning)
                    inventory.ThresholdWarning = inventoryUpdate.ThresholdWarning;
            }


            //Check the status. Status should adjust based on the value of threshold critical and warning.

            
                var statusName = "";

                if(inventory.Quantity == 0)
                {
                    statusName = "No Stock";
                }
                else if (inventory.Quantity <= inventoryUpdate.ThresholdWarning 
                    && inventory.Quantity > inventoryUpdate.ThresholdCritical && inventory.Quantity > 0)
                {
                    statusName = "Warning";
                }
                else if (inventory.Quantity < inventoryUpdate.ThresholdCritical)
                {
                    statusName = "Critical";

                }
                else
                {
                    statusName = "OK";
                }

                var status = await _invRepo.GetInventoryStatusByName(statusName);

                if (status == null)
                {
                    ModelState.AddModelError("error", "Status Does not Exist or Table is Empty");
                }
                else
                {
                    inventory.Status = status;
                }


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _invRepo.Save();

            var invetoryToReturn = _mapper.Map<InventoryDetailDto>(inventory);

            return Ok(invetoryToReturn);
            //return Ok(inventoryUpdate);

        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> ArchiveInventory(int id)
        {

            var inventory = await _invRepo.GetInventory(id);

            if (inventory == null)
                return NotFound();


            inventory.IsArchived = true;

            if (await _invRepo.Save())
                return Ok();


            return BadRequest(new { error = "Error Deleting" });


        }


    }
}