using Microsoft.AspNetCore.Mvc;
using Oblig1.DAL;
using Oblig1.Models;
using Oblig1.ViewModeller;

namespace Oblig1.Controllers
{
    public class HusController : Controller
    {
        private readonly ILogger _Brukerlogger;

        private readonly HusRepo _HusRepo;


        public HusController(HusRepo husRepo)
        {
            _HusRepo = husRepo;

        }

        public async Task<IActionResult> Tabell()
        {
            var liste = await _HusRepo.HentAlle();
            if (liste == null)
            {
                _Brukerlogger.LogError("[HusController] hus liste ikke funnet dersom hentAlle() ble kalt");
                return NotFound("hus liste ikke funnet");

            }


            var itemListViewModel = new ItemListViewModel(liste, "Tabell");
            return View(itemListViewModel);

        }

        public async Task<IActionResult> tilgjengeligHus()
        {
            var liste = await _HusRepo.HentAlle();
            if (liste == null)
            {
                _Brukerlogger.LogError("[HusController] hus liste ikke funnet dersom hentAlle() ble kalt");
                return NotFound("hus liste ikke funnet");

            }


            var itemListViewModel = new ItemListViewModel(liste, "Grid");
            return View(itemListViewModel);

        }


        public async Task<IActionResult> Oversikt(int id)
        {
            var hus = await _HusRepo.hentHusMedId(id);
            if (hus == null)
            {
                _Brukerlogger.LogError("[HusController] hus var ikke funnet ved bruk av denne iden : " + id);
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
            var hus = await _HusRepo.hentHusMedId(id);
            if (hus == null)
            {
                _Brukerlogger.LogError("[HusController] hus ikke funnet ved bruke av dette brukernavnet : " + id);
                return NotFound("hus ikke funnet");
            }
            return View(hus);

        }


        [HttpPost]
        public async Task<IActionResult> endreHus(Hus hus)
        {
            if (ModelState.IsValid)
            {
                bool OK = await _HusRepo.Endre(hus);
                if (OK)
                {
                    return RedirectToAction(nameof(Tabell));
                }

            }

            _Brukerlogger.LogWarning("[HusController] endring av hus failet" + hus);
            return View(hus);



        }

        [HttpGet]
        public async Task<IActionResult> slettHus(int id)
        {
            var hus = await _HusRepo.hentHusMedId(id);
            if (hus == null)
            {
                _Brukerlogger.LogError("[HusController] bruker ikke funnet ved bruke av dette brukernavnet : " + id);
                return NotFound("hus ikke funnet");
            }
            return View(hus);

        }

        [HttpPost]
        public async Task<IActionResult> slettHusBekreftet(int id)
        {


            bool OK = await _HusRepo.Slett(id);
            if (!OK)
            {
                _Brukerlogger.LogWarning("[HusController] sletting av bruker failet" + id);
                return BadRequest("Sletting av hus mislyktes");
            }
            return RedirectToAction($"{nameof(Tabell)}");

        }





    }
}

