using Microsoft.AspNetCore.Mvc;
using Oblig1.DAL;
using Oblig1.Models;
using Oblig1.ViewModeller;


namespace Oblig1.Controllers
{
    public class OrdreController : Controller
    {
        private readonly ILogger _Ordrelogger;
        private readonly OrdreRepo _ordreRepo;

        public OrdreController(OrdreRepo ordreRepo, ILogger<OrdreController> logger)
        {
            _ordreRepo = ordreRepo;
            _Ordrelogger = logger;
        }

        public async Task<IActionResult> Tabell()
        {

            var liste = await _ordreRepo.HentAlle();
            if (liste == null)
            {
                _Ordrelogger.LogError("[OrdreController] ordre liste ikke funnet dersom _ordreRepo.HentAlle");
                return NotFound("Ordre liste ikke funnet");
            }

            var ItemListViewModel = new ItemListViewModel(liste, "Tabell");
            return View(ItemListViewModel);
        }
        [HttpGet]
        public async Task<IActionResult> Endre(int id)
        {
            var ordre = await _ordreRepo.hentOrdreMedId(id);
            if (ordre == null)
            {
                _Ordrelogger.LogError("[OrdreKontroller] Ordre ikke funnet for denne iden" + id);
                return NotFound("Ordre ikke funnet");
            }
            return View(ordre);
        }

        public async Task<IActionResult> EndreBekreftet(Ordre ordre)
        {
            if (ModelState.IsValid)
            {
                bool OK = await _ordreRepo.endreOrdre(ordre);
                if (OK)
                {
                    return RedirectToAction(nameof(Tabell));
                }

            }

            _Ordrelogger.LogWarning("[OrdreKontroller] oppdatering av ordre failet", ordre);
            return View(ordre);



        }

        [HttpGet]
        public IActionResult Create() { return View(); }


        [HttpPost]
        public async Task<IActionResult> Lag(Ordre ordre)
        {
            if (ModelState.IsValid)
            {
                bool OK = await _ordreRepo.lagOrdre(ordre);
                if (OK)
                {
                    return RedirectToAction(nameof(Tabell));

                }
            }
            _Ordrelogger.LogWarning("[KundeController] Kunde laging har failet", ordre);
            return View(ordre);
        }

        [HttpGet]

        public async Task<IActionResult> Slett(int id)
        {
            var ordre = await _ordreRepo.hentOrdreMedId(id);
            if (ordre == null)
            {
                _Ordrelogger.LogError("[OrdreKontroller] Ordre ikke funnet for denne iden", id);
                return BadRequest("Ordre ikke funnet for id gitt");

            }
            return View(ordre);

        }

        [HttpPost]

        public async Task<IActionResult> SlettBekreftet(int id)
        {
            bool OK = await _ordreRepo.SlettOrdre(id);
            if (!OK)
            {
                _Ordrelogger.LogError("[OrdreKontroller] sletting av bruker mislyktes for denne iden", id);
                return BadRequest("sletting av ordre failet");
            }
            return RedirectToAction(nameof(Tabell));
        }
    }

}
