using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MVCWebApp.Controllers
{
    [Authorize(Roles = "ea2535b6-a1ba-4fc6-885a-f02583a45af6")]
    public class SecuredController : Controller
    {
        public IActionResult Index()
        {
           return View();
        }
        
    }
}
