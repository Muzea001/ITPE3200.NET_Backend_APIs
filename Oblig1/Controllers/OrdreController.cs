using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Oblig1.DAL;
using Oblig1.Models;
using Oblig1.Services;
using Oblig1.ViewModeller;



namespace Oblig1.Controllers
{
    [Route("api/Ordre")]
    [ApiController]
    public class OrdreController : Controller
    {
        private readonly UserManager<Person> _userManager;    
        private readonly ILogger _Ordrelogger;
        private readonly OrdreInterface _ordreInterface;
        private readonly Kvittering _kvittering;
        private readonly HusInterface _husInterface;
        private readonly KundeInterface _kunderinterface;
        private readonly PersonInterface _personInterface;
        private readonly ItemDbContext _db;
        

        public OrdreController(OrdreInterface ordreinterface, ILogger<OrdreController> logger, HusInterface husInterface, PersonInterface personInterface, Kvittering kvittering, ItemDbContext dbContext, KundeInterface kundeInterface, UserManager<Person> userManager)
        {
            _userManager = userManager;
            _ordreInterface = ordreinterface;
            _Ordrelogger = logger;
            _husInterface = husInterface; ;
            _kvittering = kvittering;
            _kunderinterface = kundeInterface;
            _db = dbContext;
            _personInterface = personInterface;
            
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("Tabell")]
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


       
        [HttpGet("HentMine/{id}")]
        public async Task <IActionResult> HentMine(string id)
        {

            var Ordre = await _ordreInterface.HentMine(id);
            if (Ordre == null)
            {
                _Ordrelogger.LogError("[OrdreController] ordre liste ikke funnet for denne iden" + id);
                return NotFound();
            }

            return Ok(Ordre);



        }


        [Authorize(Roles = "Admin")]
        [HttpGet("Endre/{id}")]
        
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

        [Authorize(Roles = "Admin")]
        [HttpPost("EndreBekreftet")]
        
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

        [Authorize(Roles = "Admin, Bruker")]
        [HttpGet("lagOrdre/{id}")]
        
        public async Task <IActionResult> lagOrdre(int id, string brukerId) {
            
            var kunde = await _kunderinterface.hentKundeMedPersonId(brukerId);
            var hus = await _husInterface.hentHusMedId(id);
            if (hus == null || kunde==null) {
                return RedirectToAction("Error");
          }
           
            return Ok();
        }

        [HttpGet("SjekkTilgjengelighet")]

        public async Task<IActionResult> sjekkTilgjengelighet(int husId, DateTime startDato, DateTime sluttDato)
        {

            bool OK = await _ordreInterface.sjekkTilgjengelighet(husId, startDato, sluttDato);
            if(OK) {
                return Json(OK);
                    }

            else
            {
                return Json(!OK);
            }

            
        
        
        
        
        }


        [Authorize(Roles = "Admin, Bruker")]
        [HttpPost("Lag")]
        public async Task<IActionResult> Lag(DateTime startDato, DateTime sluttDato, string betaltGjennom, int husID, decimal fullPrice)
        {
            try
            {
                _Ordrelogger.LogInformation("Entering Lag method with parameters: startDato={startDato}, sluttDato={sluttDato}, betaltgjennom={betaltgjennom}, husID={husID} , fullPrice = {fullPrice}" ,
                                             startDato, sluttDato, betaltGjennom, husID, fullPrice);

                Ordre ordre = new Ordre
                {
                    fullPris = fullPrice,
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
                    return View("Error", ModelState); 
                }

                var personID = _userManager.GetUserId(User);
                var person = await _userManager.FindByIdAsync(personID);
                var ordreHus = await _husInterface.hentHusMedId(husID);
                var existingKunde = await _db.Kunde.FirstOrDefaultAsync(k => k.Person.Id == personID);
                Kunde kunde;

                if (existingKunde != null)
                {
                    existingKunde.ordreListe = new List<Ordre>();
                    existingKunde.husListe = new List<Hus>();
                    kunde = existingKunde;
                }
                else
                {
                   
                    kunde = new Kunde { Person = person, husListe = new List<Hus>(), ordreListe = new List<Ordre>() };
                    _db.Kunde.Add(kunde); 
                }

                
                ordre.hus = ordreHus;
                ordre.kunde = kunde;

                    bool OK = await _ordreInterface.lagOrdre(ordre);
                    if (OK)
                    {
                        ordre.kunde.ordreListe.Add(ordre);
                        ordre.kunde.husListe.Add(ordreHus);
                        ordre.hus.ordreListe.Add(ordre);
                        return View("Kvittering", ordre);
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

        }

        [HttpGet("regnFullPris")]
        public async Task<JsonResult> regnFullPris(DateTime start, DateTime slutt, decimal Pris)
        {
            

            decimal days = (slutt - start).Days;

            decimal fullPris = Pris * days;

            await Task.Delay(1);

            return new JsonResult(fullPris);
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("Slett/{id}")]
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

        [Authorize(Roles = "Admin")]
        [HttpPost("SlettBekreftet/{id}")]
        
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
