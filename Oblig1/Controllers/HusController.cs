using Microsoft.AspNetCore.Mvc;
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
            Interface = husInterface;
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
            var liste = await husInterface.hentAlleMedFilter( by,minstAreal,maksAreal,minPris, maksPris,  minstRom,maksRom);
            if(liste==null)
            {
                return NotFound("ingenting funnet");
            }

            var itemListViewModel = new ItemListViewModel(liste, "Tabell");
            return View(itemListViewModel);

        }


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
        public async Task<IActionResult> endreHus(Hus hus)
        {
            if (ModelState.IsValid)
            {
                bool OK = await husInterface.Endre(hus);
                if (OK)
                {
                    return RedirectToAction(nameof(Tabell));
                }

            }

            _HusLogger.LogWarning("[HusController] endring av hus failet" + hus);
            return View(hus);



        }

        [HttpGet]
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

