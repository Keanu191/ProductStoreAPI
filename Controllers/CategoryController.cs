using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using WebApplicationDemoS4.Models;

namespace WebApplicationDemoS4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ShopContext _shopContext;
        private readonly IMongoCollection<Category> _categories;

        public CategoryController(ShopContext shopContext, IMongoClient mongoClient)
        {
            _shopContext = shopContext;
            _shopContext.Database.EnsureCreated();

            // for mongo
            var database = mongoClient.GetDatabase("MongoDb"); // this calls the connection string to my datbabase in the appsettings json file
            _categories = database.GetCollection<Category>("Category"); // one of the collection name in my db
        }

        // Get
        [HttpGet]
        public async Task<ActionResult> GetAllProducts()
        {
            var category = await _shopContext.Category.ToListAsync();
            return Ok(category);
        }

        [HttpGet, Route("get")]
        public async Task<ActionResult> GetCategory(int id)
        {
            var category = await _shopContext.Category.FindAsync(id);
            // here we use Ok product
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        // Post
        [HttpPost]

        public async Task<ActionResult<Product>> PostCategory(Category category)
        {
            _shopContext.Category.Add(category);
            await _shopContext.SaveChangesAsync();
            return CreatedAtAction("GetProduct", new { id = category.Id }, category);
        }

        // Put
        [HttpPut("{id}")]
        public async Task<ActionResult> PutCategory(int id, [FromBody] Category category)
        {
            // here we are updating our data store
            _shopContext.Entry(category).State = EntityState.Modified;

            try
            {
                await _shopContext.SaveChangesAsync();
            }
            // maybe the product has been modified already
            catch (DbUpdateConcurrencyException ex)
            {
                if (!_shopContext.Category.Any(p => p.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            await _shopContext.SaveChangesAsync();

            return NoContent();
        }
        // delete
        [HttpDelete("{id}")]
        public async Task<ActionResult<Category>> DeleteCategory(int id)
        {
            var category = await _shopContext.Category.FindAsync(id);
            if (category == null)
            {
                // this will be the error 404 response
                return NotFound();
            }
            _shopContext.Category.Remove(category);
            await _shopContext.SaveChangesAsync();

            return category;
        }
    }
}
