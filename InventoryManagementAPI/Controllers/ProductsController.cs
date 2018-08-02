﻿using System;
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
    [Route("api/products")]
    public class ProductsController : Controller
    {

        private readonly IProductRepository _productRepo;
        private readonly IMapper _mapper;

        public ProductsController(IProductRepository productRepo, IMapper mapper)
        {
            _productRepo = productRepo;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] ProductParams productParams)
        {
            var products = await _productRepo.GetProducts(productParams);

            var productsInPage = _mapper.Map<ICollection<ProductListDto>>(products);

            Response.AddPagination(products.CurrentPage, products.PageSize, products.TotalCount, products.TotalPages);

            return Ok(productsInPage);

        }


        [HttpGet("{id}", Name = "GetProduct")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _productRepo.GetProduct(id);

            if (product == null)
                return NotFound();

            var productDetail = _mapper.Map<ProductDetailDto>(product);

            return Ok(productDetail);

        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreateDto newProduct)
        {
            var productCategory = await _productRepo.GetProductCategory(newProduct.ProductCategoryId);

            if (productCategory == null)
                ModelState.AddModelError("ProductCategory", "Invalid Product Category");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Product createdProduct = new Product
            {
                Upc = newProduct.Upc,
                Name = newProduct.Name,
                Descriptions = newProduct.Descriptions,
                ProductCategory = productCategory
            };

            _productRepo.Add(createdProduct);

            if(await _productRepo.Save())
            {

                var productToReturn = _mapper.Map<ProductDetailDto>(createdProduct);

                return CreatedAtRoute("GetProduct", new { controller = "Products", id = createdProduct.Id }, productToReturn);
            }

            return BadRequest();

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductCreateDto productUpdate)
        {
            var product = await _productRepo.GetProduct(id);

            if (product == null)
                return NotFound();

            var category = await _productRepo.GetProductCategory(productUpdate.ProductCategoryId);

            if (category == null)
                ModelState.AddModelError("ProductCategory", "Product Category Not Found");


            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            product.Upc = productUpdate.Upc;
            product.Name = productUpdate.Name;
            product.Descriptions = productUpdate.Descriptions;
            product.ProductCategory = category;


            if(await _productRepo.Save())
            {
                var updatedProduct = _mapper.Map<ProductDetailDto>(product);

                return Ok(updatedProduct);
            }

            return BadRequest(new { error = "Product cannot be updated" });


        }


        [HttpGet("categories")]
        public async Task<IActionResult> GetProductCategories()
        {
            var productCategories = await _productRepo.GetProductCategories();

            var returnedCategories = _mapper.Map<ICollection<ProductCategoryInfoDto>>(productCategories);


            return Ok(returnedCategories);

        }

        [HttpPost("categories")]
        public async Task<IActionResult> CreateProductCategories([FromBody] ProductCategoryFormDto newCategory)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ProductCategory category = new ProductCategory
            {
                Name = newCategory.Name
            };

            _productRepo.Add(category);

            if(await _productRepo.Save())
            {
                return StatusCode(201);
            }

            return BadRequest(new { error = "Product Category cannot be saved" });

        }

        [HttpPut("categories/{id}")]
        public async Task<IActionResult> UpdateProductCategories(int id, [FromBody] ProductCategoryFormDto categoryUpdate)
        {
            var category = await _productRepo.GetProductCategory(id);

            if (category == null)
                return NotFound();

            category.Name = categoryUpdate.Name;

            if(await _productRepo.Save())
            {
                var categoryToReturn = _mapper.Map<ProductCategoryInfoDto>(category);

                return Ok(categoryToReturn);
            }

            return BadRequest(new { error = "Cannot update category" });


        }


    }
}