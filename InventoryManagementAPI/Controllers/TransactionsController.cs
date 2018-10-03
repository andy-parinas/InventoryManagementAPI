using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using InventoryManagementAPI.Data;
using InventoryManagementAPI.Dto;
using InventoryManagementAPI.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/transactions")]
    public class TransactionsController : Controller
    {

        private readonly ITransactionRepository _transRepo;
        private readonly IInventoryRepository _invRepo;
        private readonly IMapper _mapper;

        public TransactionsController(ITransactionRepository transRepo, IInventoryRepository invRepo, IMapper mapper)
        {
            _transRepo = transRepo;
            _invRepo = invRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetInventories([FromQuery] TransactionParams transParams)
        {
            var transactions = await _transRepo.GetInventoryTransactions(transParams);

            var transactionToReturn = _mapper.Map<ICollection<TransactionListDto>>(transactions);

            Response.AddPagination(transactions.CurrentPage, transactions.PageSize, 
                transactions.TotalCount, transactions.TotalPages);

            return Ok(transactionToReturn);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetInventory(int id)
        {
            var transaction = await _transRepo.GetInventoryTransaction(id);

            if (transaction == null)
                return NotFound();


            var transactionToReturn = _mapper.Map<TransactionDetailDto>(transaction);

            return Ok(transactionToReturn);


        }

        [HttpGet("types")]
        public async Task<IActionResult> GetTransactionTypes()
        {
            var transactionTypes = await _transRepo.GetTransactionTypes();

            var typesToReturn = _mapper.Map<ICollection<TransactionTypeDto>>(transactionTypes);

            return Ok(typesToReturn);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> ArchiveInventoryTransaction(int id)
        {
            var transaction = await _transRepo.GetInventoryTransaction(id);

            if (transaction == null)
                return NotFound(new { error = new string[] { "Transaction Not Found" } });


            var inventory = await _invRepo.GetInventory(transaction.Inventory.Id);

            if(inventory == null)
                return NotFound(new { error = new string[] { "Inventory Not Found" } });

            //Check the transaction Type and revert the transaction that is being deleted
            //If transaction type is add, then subtract the transaction quantity to the inventory quantity
            //if transaction type is subtract, then add the transaction quantity to the inventory quantity
            if (string.Equals(transaction.TransactionType.Action, "add"))
                inventory.Quantity -= transaction.Quantity;

            if (string.Equals(transaction.TransactionType.Action, "subtract"))
                inventory.Quantity += transaction.Quantity;

            //Set Inventory status
            if(inventory.Quantity == 0)
            {
                var status = await _invRepo.GetInventoryStatusByName("No Stock");
                inventory.Status = status;

            }else if(inventory.Quantity <= inventory.ThresholdCritical)
            {
                var status = await _invRepo.GetInventoryStatusByName("Critical");
                inventory.Status = status;

            }else if(inventory.Quantity > inventory.ThresholdCritical && inventory.Quantity <= inventory.ThresholdWarning)
            {
                var status = await _invRepo.GetInventoryStatusByName("Warning");
                inventory.Status = status;

            }else if(inventory.Quantity > inventory.ThresholdWarning)
            {
                var status = await _invRepo.GetInventoryStatusByName("OK");
                inventory.Status = status;
            }


            //Archive the transaction
            transaction.IsArchived = true;


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

    }
}