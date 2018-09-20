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
        private readonly IProductRepository _prodctRepo;
        private readonly ILocationRepository _locationRepo;
        private readonly ITransactionRepository _transRepo;
        private readonly IMapper _mapper;


        public InventoriesController(IInventoryRepository invRepo, IMapper mapper, 
            IProductRepository productRepo, 
            ILocationRepository locationRepo, ITransactionRepository transRepo )
        {
            _invRepo = invRepo;
            _prodctRepo = productRepo;
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

        [HttpGet("statuses")]
        public async Task<IActionResult> GetInventoryStatuses()
        {
            var statuses = await _invRepo.GetInventoryStatuses();

            var statusesToReturn = _mapper.Map<ICollection<InventoryStatusListDto>>(statuses);

            return Ok(statusesToReturn);
        }


        [HttpPost]
        public async Task<IActionResult> CreateInventory([FromBody] InventoryCreateDto inventory)
        {
            //var product = await _prodctRepo.GetProduct(inventory.ProductId);
            var product = await _prodctRepo.GetProductByName(inventory.Product);

            if (product == null)
                ModelState.AddModelError("Product", "Product Not Found");

            var location = await _locationRepo.GetLocationByName(inventory.Location);

            if (location == null)
                ModelState.AddModelError("Location", "Location not Found");

            var status = await _invRepo.GetInventoryStatusByName(inventory.Status);

            if (status == null)
                ModelState.AddModelError("Status", "Status Not Found");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            Inventory newIventory = new Inventory
            {
                Sku = inventory.Sku,
                Quantity = 0,
                ThresholdWarning = inventory.ThresholdWarning,
                ThresholdCritical = inventory.ThresholdCritical,
                Product = product,
                Location = location,
                Status = status
            };

            _invRepo.Add(newIventory);

            if(await _invRepo.Save())
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

            if(inventory.Sku != inventoryUpdate.Sku)
            {
                //Check if SKU is existing
                var inventoryBySku = await _invRepo.GetInventoryBySku(inventoryUpdate.Sku);

                //IF SKU is not existing - update the sku. Else return Error that SKU already exist.
                //This means that the combination of Product and Location already exist.
                if(inventoryBySku == null)
                {
                    inventory.Sku = inventoryUpdate.Sku;
                }else
                {
                    ModelState.AddModelError("SKU", "SKU Already Exist");
                }

            }


            if(inventory.Product.Name != inventoryUpdate.Product)
            {
                var product = await _prodctRepo.GetProductByName(inventoryUpdate.Product);

                if (product == null)
                {
                    ModelState.AddModelError("Product", "Product Does not Exist");
                }
                else
                {
                    inventory.Product = product;
                }
                                  
            }

            if(inventory.Location.Name != inventoryUpdate.Location)
            {
                var location = await _locationRepo.GetLocationByName(inventoryUpdate.Location);

                if (location == null)
                {
                    ModelState.AddModelError("Location", "Location Does not Exist");

                }else
                {
                    inventory.Location = location;
                }
                    
                
            }
            
            if(inventoryUpdate.ThresholdCritical >= inventoryUpdate.ThresholdWarning)
            {
                ModelState.AddModelError("Threshold", "Critical should be less than the Warning");

            }else
            {
                if (inventory.ThresholdCritical != inventoryUpdate.ThresholdCritical)
                    inventory.ThresholdCritical = inventoryUpdate.ThresholdCritical;

                if (inventory.ThresholdWarning != inventoryUpdate.ThresholdWarning)
                    inventory.ThresholdWarning = inventoryUpdate.ThresholdWarning;
            }

            
            //Check the status. Status should adjust based on the value of threshold critical and warning.

            if(inventory.Quantity > 0)
            {
                var statusName = "";

                if(inventory.Quantity < inventoryUpdate.ThresholdWarning && inventory.Quantity > inventoryUpdate.ThresholdCritical)
                {
                    statusName = "Warning";
                }else if(inventory.Quantity < inventoryUpdate.ThresholdCritical)
                {
                    statusName = "Critical";

                }else
                {
                    statusName = "OK";
                }

                var status = await _invRepo.GetInventoryStatusByName(statusName);

                if(status == null)
                {
                    ModelState.AddModelError("Status", "STatus Does not Exist or Table is Empty");
                }else
                {
                    inventory.Status = status;
                }


            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _invRepo.Save();

            var invetoryToReturn = _mapper.Map<InventoryDetailDto>(inventory);

            return Ok(invetoryToReturn);


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