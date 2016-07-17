using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Pantry.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View(new { Username = "Billy bob thorton" });
        }
    }
}
