using Microsoft.AspNetCore.Mvc;
using Oblig1.DAL;
using Oblig1.Models;
using Oblig1.ViewModeller;

namespace Oblig1.Controllers
{
    public class KundeController : Controller
    {
        private readonly ILogger _Brukerlogger;
        private readonly KundeInterface _kundeInterface;

        public KundeController(KundeInterface Interface, ILogger<KundeController> logger)
        {
            _kundeInterface = Interface;
            _Brukerlogger = logger;
        }

        public async Task<IActionResult> Tabell()
        {

            var liste = await _kundeInterface.HentAlle();
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
            var kunde = await _kundeInterface.hentKundeMedId(id);
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
                bool OK = await _kundeInterface.endreKunde(kunde);   
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
                bool OK = await _kundeInterface.lagKunde(kunde);
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
            var kunde = await _kundeInterface.hentKundeMedId(id);
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
            bool OK = await _kundeInterface.SlettKunde(id);
            if (!OK)
            {
                _Brukerlogger.LogError("[BrukerController] sletting av bruker mislyktes for denne iden", id);
                return BadRequest("sletting av kunde failet");
            }
            return RedirectToAction(nameof(Tabell));
        }
    }


  
}
