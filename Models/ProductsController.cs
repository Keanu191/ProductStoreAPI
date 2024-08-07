using WebApplicationDemoS4;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplicationDemoS4.Models
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
        [HttpGet]

        public IEnumerable<Product> GetProducts()
        {
            return _shopContext.Products.ToArray();
        }
        /*
        [HttpGet]

        public IActionResult GetProducts()
        {
            return Ok();
        }
        */
    }
}
