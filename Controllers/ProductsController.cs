using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationDemoS4.Models;

namespace WebApplicationDemoS4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ShopContext _shopContext;

        public ProductsController(ShopContext shopContext)
        {
            _shopContext = shopContext;
            _shopContext.Database.EnsureCreated();
        }

        // Get
        [HttpGet]
        public async Task<ActionResult> GetAllProducts()
        {
            var products = await _shopContext.Products.ToListAsync();
            return Ok(products);
        }

        [HttpGet, Route("get")]
        public async Task<ActionResult> GetProduct(int id)
        {
            var product = await _shopContext.Products.FindAsync(id);
            // here we use Ok product
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        // Post
        [HttpPost]

        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _shopContext.Products.Add(product);
            await _shopContext.SaveChangesAsync();
            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        // Put
        [HttpPut("{id}")]
        public async Task<ActionResult> PutProduct(int id, [FromBody] Product product)
        {
            // here we are updating our data store
            _shopContext.Entry(product).State = EntityState.Modified;

            try
            {
                await _shopContext.SaveChangesAsync();
            }
            // maybe the product has been modified already
            catch (DbUpdateConcurrencyException ex)
            {
                if (!_shopContext.Products.Any(p => p.Id == id))
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
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            var product = await _shopContext.Products.FindAsync(id);
            if (product == null)
            {
                // this will be the error 404 response
                return NotFound();
            }
            _shopContext.Products.Remove(product);
            await _shopContext.SaveChangesAsync();

            return product;
        }
    }
}
