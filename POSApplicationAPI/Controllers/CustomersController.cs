using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using POSApplicationAPI.Data;
using POSApplicationAPI.Dto;
using POSApplicationAPI.Helpers;

namespace POSApplicationAPI.Controllers
{
    [Route("api/customers")]
    public class CustomersController : Controller
    {

        private readonly ICustomerRepository _customerRepo;
        private readonly IMapper _mapper;

        public CustomersController(ICustomerRepository customerRepo, IMapper mapper)
        {
            _customerRepo = customerRepo;
            _mapper = mapper;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetCustomers([FromQuery]CustomerParams customerParams)
        {
            var customers = await _customerRepo.GetCustomers(customerParams);

            var customersInPage = _mapper.Map<ICollection<CustomerListDto>>(customers);

            Response.AddPagination(customers.CurrentPage, customers.PageSize, customers.TotalCount, customers.TotalPages);

            return Ok(customersInPage);


        }

        [HttpGet("{id}", Name ="GetCustomer")]
        public async Task<IActionResult> GetCustomer(int id)
        {
            var customer = await _customerRepo.GetCustomer(id);

            if (customer == null)
                return NotFound();

            var customerToReturn = _mapper.Map<CustomerDetailDto>(customer);

            return Ok(customerToReturn);



        }


        [HttpGet("{id}/orders")]
        public async Task<IActionResult> GetCustomerOrders(int id, [FromQuery] PageParams pageParams)
        {
            var orders = await _customerRepo.GetCustomerOrders(id, pageParams);

            var ordersInPage = _mapper.Map<ICollection<OrderListDto>>(orders);

            Response.AddPagination(orders.CurrentPage, orders.PageSize, orders.TotalCount, orders.TotalPages);


            return Ok(ordersInPage);



        }



    }
}