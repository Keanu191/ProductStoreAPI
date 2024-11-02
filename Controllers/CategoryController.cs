using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using WebApplicationDemoS4.Data;
using WebApplicationDemoS4.Entities;

namespace ProductStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IMongoCollection<Category>? _categories;

        public CategoryController(MongoDBService mongoDBService)
        {
            _categories = mongoDBService.Database?.GetCollection<Category>("category");
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

        public async Task<ActionResult> Create(Category category)
        {
            await _categories.InsertOneAsync(category);
            return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
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
