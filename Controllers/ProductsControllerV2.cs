﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using WebApplicationDemoS4.Data;
using WebApplicationDemoS4.Entities;
using WebApplicationDemoS4.Models;

namespace WebApplicationDemoS4.Controllers
{
    [ApiVersion("2.0")]
    //[Route("api/[controller]")]
    //[ApiController]
    [Route("v{v:apiVersion}/products")]
    public class ProductsV2Controller : ControllerBase
    {
        private readonly IMongoCollection<Product>? _products;
        private readonly MongoContext _mongoContext;

        public ProductsV2Controller(MongoContext mongoContext)
        {
            _products = mongoContext.Database?.GetCollection<Product>("Products");
            _mongoContext = mongoContext;
        }

        [HttpGet]
        public async Task<IEnumerable<Product>> Get([FromQuery] ProductParameterQuery queryParameters)
        {
            var filter = Builders<Product>.Filter.Empty; // Start with an empty filter

            // Apply MinPrice filter if provided
            if (queryParameters.MinPrice != null)
            {
                filter &= Builders<Product>.Filter.Gte(p => p.Price, queryParameters.MinPrice.Value);
            }

            // Apply MaxPrice filter if provided
            if (queryParameters.MaxPrice != null)
            {
                filter &= Builders<Product>.Filter.Lte(p => p.Price, queryParameters.MaxPrice.Value);
            }

            // Apply pagination
            var productsQuery = _mongoContext.Products.Find(filter)
                                                      .Skip(queryParameters.Size * (queryParameters.Page - 1))
                                                      .Limit(queryParameters.Size);

            // Fetch data asynchronously
            var products = await productsQuery.ToListAsync();

            if (!string.IsNullOrEmpty(queryParameters.Sku))
            {
                products = products.Where(p => p.Sku == queryParameters.Sku).ToList();
            }

            if (!string.IsNullOrEmpty(queryParameters.Name))
            {
                products = products.Where(p => p.Name.ToLower().Contains(queryParameters.Name.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(queryParameters.sortBy))
            {
                if (typeof(Product).GetProperty(queryParameters.sortBy) != null)
                {
                    // Convert List<Product> to IQueryable<Product>
                    var productsQueryable = products.AsQueryable();

                    // Apply the custom sorting
                    productsQueryable = productsQueryable.OrderByCustom(queryParameters.sortBy, queryParameters.SortOrder);

                    // If you want to return the sorted list back, convert it to List again
                    products = productsQueryable.ToList();
                }
            }
            return products;

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product?>> GetById(int id)
        {
            var filter = Builders<Product>.Filter.Eq(x => x.Id, id);
            var product = _products.Find(filter).FirstOrDefault();
            return product is not null ? Ok(product) : NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> Post(Product product)
        {
            await _products.InsertOneAsync(product);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        [HttpPut]
        public async Task<ActionResult> Update(Product product)
        {
            var filter = Builders<Product>.Filter.Eq(x => x.Id, product.Id);

            await _products.ReplaceOneAsync(filter, product);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var filter = Builders<Product>.Filter.Eq(x => x.Id, id);
            await _products.DeleteOneAsync(filter);
            return Ok();
        }
    }

}

 
