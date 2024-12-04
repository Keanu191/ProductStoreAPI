/*
 * 30074191 / Keanu Farro
 * Edited Source Code from the following tutorial: https://www.youtube.com/watch?v=Gxf7zBl5Z64
 */

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using WebApplicationDemoS4.Entities;
using WebApplicationDemoS4.Models;
using WebApplicationDemoS4.Settings;

namespace WebApplicationDemoS4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        

        private readonly IMongoCollection<Category>? _categories;

        public CategoryController(MongoContext mongoContext)
        {  
             _categories = mongoContext.Database.GetCollection<Category>("Categories");
            
        }

        #region public functions
        // remove Authorise Policy block of code to allow a non logged in user to GET categories
        [HttpGet("Public")]
        public async Task<IEnumerable<Category>> publicGet()
        {
            return await _categories.Find(FilterDefinition<Category>.Empty).ToListAsync();
        }

        [HttpGet("Public{id}")]
        public async Task<ActionResult<Category?>> publicGetById(int id)
        {
            var filter = Builders<Category>.Filter.Eq(x => x.Id, id);
            var category = _categories.Find(filter).FirstOrDefault();
            return category is not null ? Ok(category) : NotFound();
        }
        #endregion
        [HttpGet]
        [Authorize(Policy = "Admin")]
        public async Task<IEnumerable<Category>> Get()
        {
            return await _categories.Find(FilterDefinition<Category>.Empty).ToListAsync();
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<Category?>> GetById(int id)
        {
            var filter = Builders<Category>.Filter.Eq(x => x.Id, id);
            var category = _categories.Find(filter).FirstOrDefault();
            return category is not null ? Ok(category) : NotFound();
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult> Post(Category category)
        {
            await _categories.InsertOneAsync(category);
            return CreatedAtAction(nameof(GetById), new {id = category.Id}, category);
        }

        [HttpPut]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult> Update(Category category)
        {
            var filter = Builders<Category>.Filter.Eq(x => x.Id, category.Id);

            await _categories.ReplaceOneAsync(filter, category);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            var filter = Builders<Category>.Filter.Eq(x => x.Id, id);
            await _categories.DeleteOneAsync(filter);
            return Ok();
        }
    }
}
