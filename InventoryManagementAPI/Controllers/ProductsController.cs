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
            var productCategory = await _productRepo.GetProductCategory(newProduct.Category);

            if (productCategory == null)
                ModelState.AddModelError("error", "Invalid Product Category");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Product createdProduct = new Product
            {
                Upc = newProduct.Upc,
                Name = newProduct.Product,
                Descriptions = newProduct.Descriptions,
                ProductCategory = productCategory,
                UpdatedAt = DateTime.Now
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
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductUpdateDto productUpdate)
        {
            var product = await _productRepo.GetProduct(id);

            if (product == null)
                return NotFound();

            var category = await _productRepo.GetProductCategory(productUpdate.Category);

            if (category == null)
                ModelState.AddModelError("error", "Product Category Not Found");


            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            product.Upc = productUpdate.Upc;
            product.Name = productUpdate.Product;
            product.Descriptions = productUpdate.Descriptions;
            product.ProductCategory = category;
            product.Cost = productUpdate.Cost;
            product.Price = productUpdate.Price;
            product.UpdatedAt = DateTime.Now;


            if(await _productRepo.Save())
            {
                var updatedProduct = _mapper.Map<ProductDetailDto>(product);

                return Ok(updatedProduct);
            }

            return BadRequest(new { error = new string[] { "Product cannot be updated" } });


        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id, [FromQuery] PageParams pageParams )
        {

            var product = await _productRepo.GetProduct(id);

            if (product == null)
                return NotFound(new { error = new string[] { "Product Not Found" } });

            var inventories = await _productRepo.GetProductInventories(product, pageParams);

            if (inventories.Count > 0)
                ModelState.AddModelError("error", "Product still have active inventories");


            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            product.IsArchived = true;

            if(await _productRepo.Save())
            {
                return Ok();
            }

            return BadRequest(new { error = new string[] { "Product cannot be deleted" } });
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
                return Ok(category);
            }

            return BadRequest(new { error = new string[] { "Product Category cannot be saved" } });

        }

        [HttpPut("categories/{id}")]
        public async Task<IActionResult> UpdateProductCategories(int id, [FromBody] ProductCategoryFormDto categoryUpdate)
        {
            var category = await _productRepo.GetProductCategory(id);

            if (category == null)
                return NotFound(new { error = new string[] { "Category not found" } });

            if (string.IsNullOrEmpty(categoryUpdate.Name))
                ModelState.AddModelError("error", "Category Name is required");


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            category.Name = categoryUpdate.Name;

            if(await _productRepo.Save())
            {
                var categoryToReturn = _mapper.Map<ProductCategoryInfoDto>(category);

                return Ok(categoryToReturn);
            }

            return BadRequest(new { error = new string[] { "Cannot update category" } });


        }


        [HttpDelete("categories/{id}")]
        public async Task<IActionResult> DeleteProductCategories(int id)
        {

            var category = await _productRepo.GetProductCategory(id);

            if (category == null)
                return NotFound(new { error = new string[] { "Product Category not Found" } });


            if (category.Products.Count > 0)
                return BadRequest(new { error = new string[] { "Category have associated products. Cannot be deleted" } });


            _productRepo.Delete(category);


            if(await _productRepo.Save())
            {
                return NoContent();
            }


            return BadRequest(new { error = new string[] { "Error delete category" } });

        }

    }
}