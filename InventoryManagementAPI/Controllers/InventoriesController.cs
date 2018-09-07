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



        [HttpPost]
        public async Task<IActionResult> CreateInventory([FromBody] InventoryCreateDto inventory)
        {
            var product = await _prodctRepo.GetProduct(inventory.ProductId);

            if (product == null)
                ModelState.AddModelError("Product", "Product Not Found");

            var location = await _locationRepo.GetLocation(inventory.LocationId);

            if (location == null)
                ModelState.AddModelError("Location", "Location not Found");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            Inventory newIventory = new Inventory
            {
                Sku = inventory.Sku,
                Quantity = inventory.Quantity,
                ThresholdWarning = inventory.ThresholdWarning,
                ThresholdCritical = inventory.ThresholdCritical,
                Product = product,
                Location = location
            };

            _invRepo.Add(newIventory);

            if(await _prodctRepo.Save())
            {
                var invetoryToReturn = _mapper.Map<InventoryDetailDto>(newIventory);

                return CreatedAtRoute(new { controller = "inventories", id = newIventory.Id }, invetoryToReturn);

            }

            return BadRequest(new { error = "Error creating inventory" });

        }

        /*
         * //Removing this controller action.
         * //Inventory Quantity should only be updated by creating transactions.
        [HttpPatch("sku/{sku}")]
        public async Task<IActionResult> UpdateInventoryCount(string sku, [FromBody] InventoryCountDto inventoryCount)
        {
            var inventory = await _invRepo.GetInventoryBySku(sku);

            if (inventory == null)
                return NotFound();

            int qty = inventory.Quantity + inventoryCount.Count;

            if (qty >= 0)
                inventory.Quantity = qty;

            if(await _invRepo.Save())
            {
                return Ok(inventory);
            }

            return BadRequest(new { error = "Error updating the inventory" });

        }
        */

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInventory(int id, [FromBody] InventoryUpdateDto inventoryUpdate)
        {
            var inventory = await _invRepo.GetInventory(id);

            if (inventory == null)
                return NotFound();


            if(inventory.Product.Id != inventoryUpdate.ProductId)
            {
                var product = await _prodctRepo.GetProduct(inventoryUpdate.ProductId);

                if (product == null)
                    return BadRequest(new { error = "Product not Found" });

                inventory.Product = product;
            }

            if(inventory.Location.Id != inventoryUpdate.LocationId)
            {
                var location = await _locationRepo.GetLocation(inventoryUpdate.LocationId);

                if (location == null)
                    return BadRequest(new { error = "Location not Found" });

                inventory.Location = location;
            }

            if (!string.IsNullOrEmpty(inventoryUpdate.Sku))
                inventory.Sku = inventoryUpdate.Sku;

            //Removing Inventory Update of quantity.
            //Quantity should be updated by adding transactions
            //if (inventory.Quantity != inventoryUpdate.Quantity)
            //    inventory.Quantity = inventoryUpdate.Quantity;

            if (inventory.ThresholdCritical != inventoryUpdate.ThresholdCritical)
                inventory.ThresholdCritical = inventoryUpdate.ThresholdCritical;


            await _invRepo.Save();

            return Ok(inventory);


        }



    }
}