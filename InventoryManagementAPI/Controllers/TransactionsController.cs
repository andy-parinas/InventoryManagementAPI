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
        private readonly IMapper _mapper;

        public TransactionsController(ITransactionRepository transRepo, IMapper mapper)
        {
            _transRepo = transRepo;
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



    }
}