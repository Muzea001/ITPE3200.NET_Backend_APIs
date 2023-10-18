using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Oblig1.DAL;
using Oblig1.Models;
using Oblig1.Services;
using Oblig1.ViewModeller;


namespace Oblig1.Controllers
{
    public class OrdreController : Controller
    {
        private readonly ILogger _Ordrelogger;
        private readonly OrdreInterface _ordreInterface;
        private readonly Kvittering _kvittering;
        private readonly HusInterface _husInterface;
        private readonly KundeInterface _kunderinterface;

        public OrdreController(OrdreInterface ordreinterface, ILogger<OrdreController> logger, HusInterface husInterface, Kvittering kvittering, KundeInterface kundeInterface)
        {
            _ordreInterface = ordreinterface;
            _Ordrelogger = logger;
            _husInterface = husInterface; ;
            _kvittering = kvittering;
            _kunderinterface = kundeInterface;
        }

        public async Task<IActionResult> Tabell()
        {

            var liste = await _ordreInterface.HentAlle();
            if (liste == null)
            {
                _Ordrelogger.LogError("[OrdreController] ordre liste ikke funnet dersom _ordreRepo.HentAlle");
                return NotFound("Ordre liste ikke funnet");
            }

            var ItemListViewModel = new ItemListViewModel(liste, "Tabell");
            return View(ItemListViewModel);
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Endre(int id)
        {
            var Ordre = await _ordreInterface.hentOrdreMedId(id);
            if (Ordre == null)
            {
                _Ordrelogger.LogError("[OrdreKontroller] Ordre ikke funnet for denne iden" + id);
                return NotFound("Ordre ikke funnet");
            }
            return View(Ordre);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EndreBekreftet(Ordre ordre)
        {
            if (ModelState.IsValid)
            {
                bool OK = await _ordreInterface.endreOrdre(ordre);
                if (OK)
                {
                    return RedirectToAction(nameof(Tabell));
                }
                else
                {
                    _Ordrelogger.LogWarning("[OrdreKontroller] oppdatering av ordre failet", ordre);
                    // Assuming hus has a property Id
                    ModelState.AddModelError(string.Empty, "Failed to modify the Order. Please try again.");
                }

            }
            else
            {
                _Ordrelogger.LogWarning("[OrdreKontroller] Invalid model state.", ordre);
                // Log more details about the model state errors if needed
                foreach (var modelStateKey in ViewData.ModelState.Keys)
                {
                    var modelStateVal = ViewData.ModelState[modelStateKey];
                    foreach (var error in modelStateVal.Errors)
                    {
                        // Log your modelState errors
                        _Ordrelogger.LogWarning($"Key: {modelStateKey}, Error: {error.ErrorMessage}");
                    }
                }
            }

            
            return RedirectToAction($"{nameof(Tabell)}");




        }

        [HttpGet]
        [Authorize]
        public IActionResult lagOrdre(int husId) { return View(); }


        [HttpPost]

        public async Task<IActionResult> Lag(Ordre ordre, int husid, Kunde kunde)
        {
            if (ModelState.IsValid)
            {
                var dbKunde = await _kunderinterface.hentKundeMedId(kunde.kundeID);
                if (dbKunde == null)
                {
                    await _kunderinterface.lagKunde(dbKunde);
                }
              
                ordre.kundeID = dbKunde.kundeID;
                var hus = await _husInterface.hentHusMedId(husid);
                if (hus == null)
                {
                    return NotFound("hus finnes ikke !");
                }
                ordre.husId = hus.husId;
                
                bool OK = await _ordreInterface.lagOrdre(ordre);
                if (OK)
                {
                   
                    var htmlKvittering = "<html><body><p><Kvittinerg Detaljer>.....</p></body></html>";
                    var pdfKvittering = _kvittering.genererPdfKvittering(htmlKvittering);
                    var filnavn = "Bestilling kvittering.pdf";
                    return File(pdfKvittering, filnavn);

                }
            }
            _Ordrelogger.LogWarning("[OrdreRepo] har failet med å danne en kvittering for denne ordren", ordre);

            return RedirectToAction("Review");

            

        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Slett(int id)
        {
            var ordre = await _ordreInterface.hentOrdreMedId(id);
            if (ordre == null)
            {
                _Ordrelogger.LogError("[OrdreKontroller] Ordre ikke funnet for denne iden", id);
                return BadRequest("Ordre ikke funnet for id gitt");

            }
            return View(ordre);

        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SlettBekreftet(int id)
        {
            bool OK = await _ordreInterface.SlettOrdre(id);
            if (!OK)
            {
                _Ordrelogger.LogError("[OrdreKontroller] sletting av bruker mislyktes for denne iden", id);
                return BadRequest("sletting av ordre failet");
            }
            return RedirectToAction(nameof(Tabell));
        }


    }

}
