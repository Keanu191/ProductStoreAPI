using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace MVCWebApp.Models
{
    [CollectionName("Role")]
    public class ApplicationRole : MongoIdentityRole<Guid>
    {
    }
}
