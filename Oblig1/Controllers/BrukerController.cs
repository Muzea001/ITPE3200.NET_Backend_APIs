using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oblig1.DAL;
using Oblig1.Models;
using Oblig1.Services;
using Oblig1.ViewModeller;

namespace Oblig1.Controllers
{
    public class BrukerController : Controller
    {
        private readonly ILogger _Brukerlogger;

        private readonly BrukerInterface _brukerInterface;

        private readonly eierInterface _eierInterface;


        public BrukerController(BrukerInterface Interface, ILogger<BrukerController> logger, eierInterface eierInterface)
        {
            _brukerInterface = Interface;
            _Brukerlogger = logger;
            _eierInterface = eierInterface;
        }



        public async Task<IActionResult> Tabell()
        {
            var liste = await _brukerInterface.HentAlle();
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
            var liste = await _eierInterface.HentAlleEiere();
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
            var liste = await _brukerInterface.HentAlleAdmins();
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
            var liste = await _brukerInterface.HentAlleNonAdmins();
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

        public async Task<IActionResult> endreBruker(int brukerid)
        {
            var bruker = await _brukerInterface.hentBrukerMedId(brukerid);
            if (bruker == null)
            {
                _Brukerlogger.LogError("[BrukerController] bruker ikke funnet ved bruke av dette brukernavnet : " , brukerid);
                return NotFound("bruker ikke funnet");
            }
            return View(bruker);    

        }


        [HttpPost]
        public async Task<IActionResult> endreBruker(Bruker bruker)
        {
            if (ModelState.IsValid)
            {
                bool OK = await _brukerInterface.endreBruker(bruker);
                if (OK) {
                    return RedirectToAction(nameof(Tabell));
                }

            }

            _Brukerlogger.LogWarning("[BrukerController] endring av bruker failet" + bruker);
            return View(bruker);



        }

        [HttpPost]

        public async Task<IActionResult> EndreBrukerStatus(int brukerId, bool nyStatus)
        {
            try
            {
                var OK = await _brukerInterface.EndreBrukerStatus(brukerId, nyStatus);

                if (OK)
                {
                    return Ok("Brukerstatus endret."); 
                }
                else
                {
                    return NotFound("Bruker ikke funnet."); 
                }
            }
            catch (Exception ex)
            {
                _Brukerlogger.LogError("[BrukerController] bruker ikke funnet ved bruke av dette brukernavnet : " , brukerId);
                return NotFound("bruker finnes ikke");
            }
        }

        [HttpGet]
        public async Task<IActionResult> slettBruker(int brukerid)
        {
            var bruker = await _brukerInterface.hentBrukerMedId(brukerid);
            if (bruker == null)
            {
                _Brukerlogger.LogError("[BrukerController] bruker ikke funnet ved bruke av dette brukernavnet : " , brukerid);
                return NotFound("bruker ikke funnet");
            }
            return View(bruker);

        }

        [HttpPost]

        public async Task<IActionResult> slettBrukerBekreftet(int brukerid)
        {
           
       
                bool OK = await _brukerInterface.Slett(brukerid);
                if (!OK)
                {
                    _Brukerlogger.LogWarning("[BrukerController] sletting av bruker failet" , brukerid);
                    return BadRequest("Sletting av bruker mislyktes");
                }
                return RedirectToAction($"{nameof(Tabell)}");   

            }

            



        }
    }




