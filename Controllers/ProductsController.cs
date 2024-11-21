using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using WebApplicationDemoS4.Data;
using WebApplicationDemoS4.Entities;
using WebApplicationDemoS4.Models;

namespace WebApplicationDemoS4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        /*
        private readonly MongoContext _mongoContext;

        public ProductsController(MongoContext mongoContext)
        {
            _mongoContext = mongoContext;
            _products = mongoContext.Database?.GetCollection<Product>("product");
        }

        // Get
        [HttpGet]
        public async Task<ActionResult> GetAllProducts([FromQuery] QueryParameters queryParameters)
        {
            var products = await _mongoContext._products
                .Find(FilterDefinition<Product>.Empty) // Get all products, btw empty filter means no filter
                .ToListAsync(); // Convert to a list 

            // Apply pagination (Skip and Take are synchronous)
            var pagedProducts = products
                .Skip(queryParameters.Size * (queryParameters.Page - 1))
                .Take(queryParameters.Size)
                .ToList();  // Convert to list after applying Skip/Take

            return Ok(pagedProducts.ToArray());
        }

        [HttpGet, Route("get")]
        public async Task<ActionResult> GetProduct(int id)
        {
            // Use without projection
            var product = await _mongoContext.Products.FindAsync(p => p.Id == id);
            // here we use Ok product
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        // Post
        [HttpPost]

        public ActionResult<Product> PostProduct(Product product)
        {
            _mongoContext.Products.InsertOneAsync(product);
            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        // Put
        [HttpPut("{id}")]
        public async Task<ActionResult> PutProduct(int id, [FromBody] Product product)
        {
            // Define a filter to match the product by ID
            var filter = Builders<Product>.Filter.Eq(p => p.Id, id);

            try
            {
               await _mongoContext.Products.ReplaceOneAsync(filter, product);
            }
            // maybe the product has been modified already
            catch (DbUpdateConcurrencyException ex)
            {
                // using a CountDocumentsAsync insteaf of the Any method that was previously used in shop context
                if (await _mongoContext.Products.CountDocumentsAsync(p => p.Id == id) == 0)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        // delete
        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            // Find the product to delete using the ID
            var filter = Builders<Product>.Filter.Eq(p => p.Id, id);
            var product = await _mongoContext.Products.Find(filter).FirstOrDefaultAsync();

            if (product == null)
            {
                // this will be the error 404 response
                return NotFound();
            }

            // Delete the product from the collection
            await _mongoContext.Products.DeleteOneAsync(filter);

            return product;
        }
        */
        private readonly IMongoCollection<Product>? _products;
        private readonly MongoContext _mongoContext;

        public ProductsController(MongoContext mongoContext)
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
