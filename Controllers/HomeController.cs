
using Microsoft.AspNetCore.Mvc;
using Oblig1.Services;
using Oblig1.ViewModeller;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Oblig1.Controllers

{
    
    public class HomeController : Controller
        
    {

        private readonly HusInterface husInterface;

        public HomeController(HusInterface husInterface)
        {
            this.husInterface = husInterface;
        }


        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var liste = await husInterface.hentAlle();
            if (liste == null)
            {
               
                return NotFound("hus liste ikke funnet");

            }


            var itemListViewModel = new ItemListViewModel(liste, "Tabell");
            return View(itemListViewModel);

        }
    }
}

