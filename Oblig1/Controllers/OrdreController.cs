using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Oblig1.DAL;
using Oblig1.Models;
using Oblig1.Services;
using Oblig1.ViewModeller;



namespace Oblig1.Controllers
{
    public class OrdreController : Controller
    {
        private readonly UserManager<Person> _userManager;    
        private readonly ILogger _Ordrelogger;
        private readonly OrdreInterface _ordreInterface;
        private readonly Kvittering _kvittering;
        private readonly HusInterface _husInterface;
        private readonly KundeInterface _kunderinterface;

        public OrdreController(OrdreInterface ordreinterface, ILogger<OrdreController> logger, HusInterface husInterface, Kvittering kvittering, KundeInterface kundeInterface, UserManager<Person> userManager)
        {
            _userManager = userManager;
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

                    ModelState.AddModelError(string.Empty, "Failed to modify the Order. Please try again.");
                }

            }
            else
            {
                _Ordrelogger.LogWarning("[OrdreKontroller] Invalid model state.", ordre);

                foreach (var modelStateKey in ViewData.ModelState.Keys)
                {
                    var modelStateVal = ViewData.ModelState[modelStateKey];
                    foreach (var error in modelStateVal.Errors)
                    {

                        _Ordrelogger.LogWarning($"Key: {modelStateKey}, Error: {error.ErrorMessage}");
                    }
                }
            }


            return RedirectToAction($"{nameof(Tabell)}");




        }

        [HttpGet]
        
        public async Task <IActionResult> lagOrdre(int id) {

            var brukerId = _userManager.GetUserId(User);
            var bruker = await _userManager.FindByIdAsync(brukerId);
            var kunde = await _kunderinterface.finnKundeId(brukerId);
            var hus = await _husInterface.hentHusMedId(id);
            if (hus == null || bruker==null) {
                return RedirectToAction("Error");
          }
            var viewModell = new MyViewModel
            {
                hus = hus,
                kunde = kunde,
                Person = bruker,
                ordre = new Ordre { }

            };
            return View(viewModell);
        }

        [HttpGet]

        public async Task<bool> sjekkTilgjengelighet(int husId, DateTime startDato, DateTime sluttDato)
        {

            bool OK = await _ordreInterface.sjekkTilgjengelighet(husId, startDato, sluttDato);
            if(OK) {
                return true;
                    }

            else
            {
                return false;
            }

            
        
        
        
        
        }



        [HttpPost]
        public async Task<IActionResult> Lag(DateTime startDato, DateTime sluttDato, string betaltGjennom, int husID)
        {
            try
            {
                _Ordrelogger.LogInformation("Entering Lag method with parameters: startDato={startDato}, sluttDato={sluttDato}, betaltgjennom={betaltgjennom}, husID={husID}" ,
                                             startDato, sluttDato, betaltGjennom, husID);

                Ordre ordre = new Ordre
                {
                    startDato = startDato,
                    sluttDato = sluttDato,
                    betaltGjennom = betaltGjennom
                };


                if (!ModelState.IsValid)
                {
                    foreach (var modelStateKey in ModelState.Keys)
                    {
                        var modelStateVal = ModelState[modelStateKey];
                        foreach (var error in modelStateVal.Errors)
                        {
                            _Ordrelogger.LogError($"Key: {modelStateKey}, Error: {error.ErrorMessage}");
                        }
                    }
                    return View("Error", ModelState); // Directly return Error view if model state is not valid
                }

                var personID = _userManager.GetUserId(User);
                var person = await _userManager.FindByIdAsync(personID);
                var lagetBruker = new Kunde { Person = person };
                lagetBruker.kundeID = await _kunderinterface.lagKunde(lagetBruker);
                var ordreHus = await _husInterface.hentHusMedId(husID);
                ordre.hus = ordreHus;
                ordre.kunde = lagetBruker;

                    bool OK = await _ordreInterface.lagOrdre(ordre);
                    if (OK)
                    {
                        var htmlKvittering = "<html><body><p>Kvittering Detaljer>.....</p></body></html>";
                        var pdfKvittering = _kvittering.genererPdfKvittering(htmlKvittering);
                        var filnavn = "Bestilling kvittering.pdf";
                        return File(pdfKvittering, "application/pdf", filnavn);
                    }
                    else
                    {
                        _Ordrelogger.LogWarning("Lagring av data ordre ikke godkjent");
                        return View("Error", ModelState);
                    }
                
               
            }
            catch (Exception ex)
            {
                _Ordrelogger.LogError(ex, "An error occurred while processing the Lag method");
                return View("Error", ex.Message);
            }

            var brukerId = _userManager.GetUserId(User);
            var bruker = await _userManager.FindByIdAsync(brukerId);
            var kunde = await _kunderinterface.finnKundeId(brukerId);
            var hus = await _husInterface.hentHusMedId(husID);

            var viewModell = new MyViewModel
            {
                hus = hus,
                kunde = kunde,
                Person = bruker,
                ordre = new Ordre { }
            };

            _Ordrelogger.LogWarning("Failed to generate a receipt for this order");
            return View("lagOrdre", viewModell);
        }

        [HttpGet]
       
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
