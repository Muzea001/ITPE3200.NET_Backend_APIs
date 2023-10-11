using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Oblig1.Models;
using Oblig1.Services;
using Oblig1.ViewModeller;

namespace Oblig1.Controllers
{
    public class HusController : Controller
    {
        private readonly ILogger _HusLogger;

        private readonly HusInterface husInterface;



        public HusController(HusInterface Interface, ILogger<HusController> logger)
        {
            husInterface = Interface;
            _HusLogger = logger;


        }

        public async Task<IActionResult> Tabell()
        {
            var liste = await husInterface.hentAlle();
            if (liste == null)
            {
                _HusLogger.LogError("[HusController] hus liste ikke funnet dersom hentAlle() ble kalt");
                return NotFound("hus liste ikke funnet");

            }


            var itemListViewModel = new ItemListViewModel(liste, "Tabell");
            return View(itemListViewModel);

        }

        public async Task<IActionResult> tilgjengeligHus()
        {
            var liste = await husInterface.hentAlle();
            if (liste == null)
            {
                _HusLogger.LogError("[HusController] hus liste ikke funnet dersom hentAlle() ble kalt");
                return NotFound("hus liste ikke funnet");

            }


            var itemListViewModel = new ItemListViewModel(liste, "Grid");
            return View(itemListViewModel);

        }

        public async Task<IActionResult> hentMedFilter(string by, int minstAreal, int maksAreal, int minPris, int maksPris, int minstRom, int maksRom)
        {
            var liste = await husInterface.hentAlleMedFilter(by, minstAreal, maksAreal, minPris, maksPris, minstRom, maksRom);
            if (liste == null)
            {
                return NotFound("ingenting funnet");
            }

            var itemListViewModel = new ItemListViewModel(liste, "Tabell");
            return View(itemListViewModel);

        }

        [HttpGet]
        public async Task<IActionResult> Oversikt(int id)
        {
            var hus = await husInterface.hentHusMedId(id);
            if (hus == null)
            {
                _HusLogger.LogError("[HusController] hus var ikke funnet ved bruk av denne iden : " + id);
                return NotFound("hus var ikke funnet");
            }

            return View(hus);


        }




        [HttpGet]
        public IActionResult Create()
        {

            return View();


        }



        [HttpGet]
        [Authorize]

        public async Task<IActionResult> endreHus(int id)
        {
            var hus = await husInterface.hentHusMedId(id);
            if (hus == null)
            {
                _HusLogger.LogError("[HusController] hus ikke funnet ved bruke av dette brukernavnet : " + id);
                return NotFound("hus ikke funnet");
            }
            return View(hus);

        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> endreBekreftet(Hus hus)
        {
                 if (ModelState.IsValid)
                    {
                   bool OK = await husInterface.Endre(hus);
                 if (OK)
                  {
                return RedirectToAction(nameof(Tabell));
                 }
                    else
            {
                _HusLogger.LogWarning("[HusController] Failed to modify the house. House ID: " );
                // Assuming hus has a property Id
                ModelState.AddModelError(string.Empty, "Failed to modify the house. Please try again.");
            }

        }
                  else
            {
                _HusLogger.LogWarning("[HusController] Invalid model state.");
                // Log more details about the model state errors if needed
            }

            // Stay on the current view and display an error message if the update fails
            return RedirectToAction($"{nameof(Tabell)}");

        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> slettHus(int id)
        {
            var hus = await husInterface.hentHusMedId(id);
            if (hus == null)
            {
                _HusLogger.LogError("[HusController] bruker ikke funnet ved bruke av dette brukernavnet : " + id);
                return NotFound("hus ikke funnet");
            }
            return View(hus);

        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> slettHusBekreftet(int id)
        {


            bool OK = await husInterface.Slett(id);
            if (!OK)
            {
                _HusLogger.LogWarning("[HusController] sletting av bruker failet" + id);
                return BadRequest("Sletting av hus mislyktes");
            }
            return RedirectToAction($"{nameof(Tabell)}");

        }





    }
}

