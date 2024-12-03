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

        #region Public
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
        #endregion

        #region Admin
        // Add admin policy to all CRUD functions and then reference it through Program.cs
        [HttpGet("ADMIN_GET")]
        [Authorize(Policy = "Admin")]
        public async Task<IEnumerable<Category>> adminGet()
        {
            return await _categories.Find(FilterDefinition<Category>.Empty).ToListAsync();
        }

        [HttpGet("Admin_{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<Category?>> adminGetById(int id)
        {
            var filter = Builders<Category>.Filter.Eq(x => x.Id, id);
            var category = _categories.Find(filter).FirstOrDefault();
            return category is not null ? Ok(category) : NotFound();
        }

        [HttpPost("ADMIN_POST")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult> adminPost(Category category)
        {
            await _categories.InsertOneAsync(category);
            return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
        }

        [HttpPut("ADMIN_PUT")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult> adminUpdate(Category category)
        {
            var filter = Builders<Category>.Filter.Eq(x => x.Id, category.Id);

            await _categories.ReplaceOneAsync(filter, category);
            return Ok();
        }

        [HttpDelete("ADMINDELETE_{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult> adminDelete(int id)
        {
            var filter = Builders<Category>.Filter.Eq(x => x.Id, id);
            await _categories.DeleteOneAsync(filter);
            return Ok();
        }
        #endregion
    }
}
