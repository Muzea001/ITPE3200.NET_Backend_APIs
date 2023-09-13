using Microsoft.AspNetCore.Mvc;
using Oblig1.DAL;
using Oblig1.Models;
using Oblig1.ViewModeller;

namespace Oblig1.Controllers
{
    public class KundeController : Controller
    {
        private readonly ILogger _Brukerlogger;
        private readonly KundeRepo _kundeRepo;

        public KundeController(KundeRepo kundeRepo, ILogger<KundeController> logger)
        {
            _kundeRepo = kundeRepo;
            _Brukerlogger = logger;
        }

        public async Task<IActionResult> Tabell()
        {

            var liste = await _kundeRepo.HentAlle();
            if (liste == null)
            {
                _Brukerlogger.LogError("[BrukerController] bruker liste ikke funnet dersom _brukerRepo.HentAlle");
                return NotFound("Bruker liste ikke funnet");
            }

            var itemListViewModel = new ItemListViewModel(liste, "Tabell");
            return View(itemListViewModel);
        }
        [HttpGet]
        public async Task<IActionResult> Endre(int id)
        {
            var kunde = await _kundeRepo.hentKundeMedId(id);
            if (kunde == null)
            {
                _Brukerlogger.LogError("[KundeKontroller] Kunde ikke funnet for denne iden" + id);
                return NotFound("Kunde ikke funnet");
            }
            return View(kunde);
        }

        public async Task<IActionResult> EndreBekreftet(Kunde kunde)
        {
            if(ModelState.IsValid)
            {
                bool OK = await _kundeRepo.endreKunde(kunde);   
                if (OK) 
                {
                    return RedirectToAction(nameof(Tabell));
                }

            }

            _Brukerlogger.LogWarning("[KundeController] oppdatering av kunde failet", kunde);
            return View(kunde);



        }

        [HttpGet]
        public IActionResult Create() { return View(); }


        [HttpPost]
        public async Task<IActionResult> Lag(Kunde kunde)
        {
            if (ModelState.IsValid)
            {
                bool OK = await _kundeRepo.lagKunde(kunde);
                if (OK)
                {
                    return RedirectToAction(nameof(Tabell));

                }
            }
            _Brukerlogger.LogWarning("[KundeController] Kunde laging har failet", kunde);
            return View(kunde);
        }

        [HttpGet]

        public async Task<IActionResult> Slett(int id)
        {
            var kunde = await _kundeRepo.hentKundeMedId(id);
             if(kunde == null)
            {
                _Brukerlogger.LogError("[KundeController] kunde ikke funnet for denne iden", id);
                return BadRequest("Kunde ikke funnet for id gitt");

            }
             return View(kunde);

        }

        [HttpPost]

        public async Task<IActionResult> SlettBekreftet(int id)
        {
            bool OK = await _kundeRepo.SlettKunde(id);
            if (!OK)
            {
                _Brukerlogger.LogError("[BrukerController] sletting av bruker mislyktes for denne iden", id);
                return BadRequest("sletting av kunde failet");
            }
            return RedirectToAction(nameof(Tabell));
        }
    }


  
}
