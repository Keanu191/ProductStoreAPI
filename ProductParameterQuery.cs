using WebApplicationDemoS4.Models;

namespace WebApplicationDemoS4
{
    public class ProductParameterQuery : QueryParameters
    {
        public decimal ? MinPrice { get; set; }

        public decimal? MaxPrice { get; set; }

        public string Sku { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
    }
}
