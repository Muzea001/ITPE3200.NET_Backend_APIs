using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oblig1.DAL;
using Oblig1.Models;
using Oblig1.ViewModeller;

namespace Oblig1.Controllers
{
    public class BrukerController : Controller
    {
        private readonly ILogger _Brukerlogger;

        private readonly BrukerRepo _brukerRepo;


        public BrukerController(BrukerRepo brukerRepo)
        {
            _brukerRepo = brukerRepo;

        }

        public async Task<IActionResult> Tabell()
        {
            var liste = await _brukerRepo.HentAlle();
            if (liste == null)
            {
                _Brukerlogger.LogError("[BrukerController] bruker liste ikke funnet dersom hentAlle() ble kalt");
                return NotFound("bruker liste ikke funnet");

            }


            var itemListViewModel = new ItemListViewModel(liste, "Tabell");
            return View(itemListViewModel);

        }

        public async Task<IActionResult> EierTabell()
        {
            var liste = await _brukerRepo.HentAlleEiere();
            if (liste == null)
            {
                _Brukerlogger.LogError("[BrukerController] eier liste ikke funnet dersom hentAlle() ble kalt");
                return NotFound("eier liste ikke funnet");

            }


            var itemListViewModel = new ItemListViewModel(liste, "EierTabell");
            return View(itemListViewModel);

        }

        public async Task<IActionResult> AdminTabell()
        {
            var liste = await _brukerRepo.HentAlleAdmins();
            if (liste == null)
            {
                _Brukerlogger.LogError("[BrukerController] admin liste ikke funnet dersom AdminTabell() ble kalt");
                return NotFound("admin liste ikke funnet");

            }


            var itemListViewModel = new ItemListViewModel(liste, "AdminTabell");
            return View(itemListViewModel);

        }

        public async Task<IActionResult> NonAdminTabell()
        {
            var liste = await _brukerRepo.HentAlleNonAdmins();
            if (liste == null)
            {
                _Brukerlogger.LogError("[BrukerController] non admin liste ikke funnet dersom AdminTabell() ble kalt");
                return NotFound("non admin liste ikke funnet");

            }


            var itemListViewModel = new ItemListViewModel(liste, "AdminTabell");
            return View(itemListViewModel);

        }

        [HttpGet]
        public IActionResult Lag()
        {
            return View();
        }



        [HttpGet]

        public async Task<IActionResult> endreBruker(string brukernavn)
        {
            var bruker = await _brukerRepo.hentBrukerMedId(brukernavn);
            if (bruker == null)
            {
                _Brukerlogger.LogError("[BrukerController] bruker ikke funnet ved bruke av dette brukernavnet : " + brukernavn);
                return NotFound("bruker ikke funnet");
            }
            return View(bruker);    

        }


        [HttpPost]
        public async Task<IActionResult> endreBruker(Bruker bruker)
        {
            if (ModelState.IsValid)
            {
                bool OK = await _brukerRepo.endreBruker(bruker);
                if (OK) {
                    return RedirectToAction(nameof(Tabell));
                }

            }

            _Brukerlogger.LogWarning("[BrukerController] endring av bruker failet" + bruker);
            return View(bruker);



        }

        [HttpGet]
        public async Task<IActionResult> slettBruker(string brukernavn)
        {
            var bruker = await _brukerRepo.hentBrukerMedId(brukernavn);
            if (bruker == null)
            {
                _Brukerlogger.LogError("[BrukerController] bruker ikke funnet ved bruke av dette brukernavnet : " + brukernavn);
                return NotFound("bruker ikke funnet");
            }
            return View(bruker);

        }

        [HttpPost]

        public async Task<IActionResult> slettBrukerBekreftet(string brukernavn)
        {
           
       
                bool OK = await _brukerRepo.Slett(brukernavn);
                if (!OK)
                {
                    _Brukerlogger.LogWarning("[BrukerController] sletting av bruker failet" + brukernavn);
                    return BadRequest("Sletting av bruker mislyktes");
                }
                return RedirectToAction($"{nameof(Tabell)}");   

            }

            



        }
    }




