using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace MVCWebApp.Models
{
    [CollectionName("User")]
    public class ApplicationUser : MongoIdentityUser<Guid>
    {
        // to prevent exception
        public string FullName { get; set; } = string.Empty;

        public string CurrentRole { get; set; } = string.Empty;
    }
}
