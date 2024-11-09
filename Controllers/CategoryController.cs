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
    public class CategoryController : ControllerBase
    {
        /*
        private readonly MongoContext _mongoContext;

        public CategoryController(MongoContext mongoContext)
        {
            _mongoContext = mongoContext;
        }

        // Get
        [HttpGet]
        public async Task<ActionResult> GetAllCategories([FromQuery] QueryParameters queryParameters)
        {
            var categories = await _mongoContext.Categories
                .Find(FilterDefinition<Category>.Empty) // Get all categories, btw empty filter means no filter
                .ToListAsync(); // Convert to a list 

            // Apply pagination (Skip and Take are synchronous)
            var pagedCategories = categories
                .Skip(queryParameters.Size * (queryParameters.Page - 1))
                .Take(queryParameters.Size)
                .ToList();  // Convert to list after applying Skip/Take

            return Ok(pagedCategories.ToArray());
        }

        [HttpGet, Route("get")]
        public async Task<ActionResult> GetCategory(int id)
        {
            // Use without projection
            var category = await _mongoContext.Categories.FindAsync(p => p.Id == id);
            // here we use Ok product
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        // Post
        [HttpPost]

        public ActionResult<Product> PostCategory(Category category)
        {
            _ = _mongoContext.Categories.InsertOneAsync(category);
            return CreatedAtAction("GetCategory", new { id = category.Id }, category);
        }

        // Put
        [HttpPut("{id}")]
        public async Task<ActionResult> PutCategory(int id, [FromBody] Category category)
        {
            // Define a filter to match the product by ID
            var filter = Builders<Category>.Filter.Eq(p => p.Id, id);

            try
            {
                await _mongoContext.Categories.ReplaceOneAsync(filter, category);
            }
            // maybe the product has been modified already
            catch (DbUpdateConcurrencyException ex)
            {
                // using a CountDocumentsAsync insteaf of the Any method that was previously used in shop context
                if (await _mongoContext.Categories.CountDocumentsAsync(p => p.Id == id) == 0)
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
        public async Task<ActionResult<Category>> DeleteCategory(int id)
        {
            // Find the category to delete using the ID
            var filter = Builders<Category>.Filter.Eq(p => p.Id, id);
            var category = await _mongoContext.Categories.Find(filter).FirstOrDefaultAsync();

            if (category == null)
            {
                // this will be the error 404 response
                return NotFound();
            }

            // Delete the product from the collection
            await _mongoContext.Categories.DeleteOneAsync(filter);

            return category;
        }
        */

        private readonly IMongoCollection<Category> _categories;

        public CategoryController(MongoContext mongoContext)
        {
            _categories = mongoContext.Database?.GetCollection<Category>("category");
        }

        [HttpGet]
        public async Task<IEnumerable<Category>> Get()
        {
            return await _categories.Find(FilterDefinition<Category>.Empty).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category?>> GetById(int id)
        {
            var filter = Builders<Category>.Filter.Eq(x => x.Id, id);
            var category = _categories.Find(filter).FirstOrDefault();
            return category is not null ? Ok(category) : NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> Post(Category category)
        {
            await _categories.InsertOneAsync(category);
            return CreatedAtAction(nameof(GetById), new {id = category.Id}, category);
        }

        [HttpPut]
        public async Task<ActionResult> Update(Category category)
        {
            var filter = Builders<Category>.Filter.Eq(x => x.Id, category.Id);

            await _categories.ReplaceOneAsync(filter, category);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var filter = Builders<Category>.Filter.Eq(x => x.Id, id);
            await _categories.DeleteOneAsync(filter);
            return Ok();
        }
    }
}
