using Microsoft.AspNetCore.Mvc;

namespace Oblig1.Controllers
{
    public class KundeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
