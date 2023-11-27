using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Oblig1.DAL;
using Oblig1.Models;
using Oblig1.Services;
using Oblig1.ViewModeller;

namespace Oblig1.Controllers
{
    [Route("api/Kunde")]
    [ApiController]
    public class KundeController : Controller
    {
        private readonly ILogger _Brukerlogger;
        private readonly KundeInterface _kundeInterface;
        private readonly PersonInterface _personInterface;

        public KundeController(KundeInterface Interface, ILogger<KundeController> logger, PersonInterface personInterface)
        {
            _personInterface = personInterface;
            _kundeInterface = Interface;
            _Brukerlogger = logger;
        }


        [HttpGet("Tabell")]
        
        public async Task<IActionResult> Tabell()
        {

            var liste = await _kundeInterface.HentAlle();
            if (liste == null)
            {
                _Brukerlogger.LogError("[BrukerController] bruker liste ikke funnet dersom _brukerRepo.HentAlle");
                return NotFound("Bruker liste ikke funnet");
            }

            var itemListViewModel = new ItemListViewModel(liste, "Tabell");
            return Ok(liste);
        }

        [HttpGet("hentMin/{id}")]
        public async Task<IActionResult> hentMin(string id) { 
        
        var kunde = await _kundeInterface.hentKundeMedPersonId(id);
            if (kunde== null)
            {
                _Brukerlogger.LogError("[KundeKontroller] Kunde ikke funnet for denne iden" + id);
                return NotFound("Kunde ikke funnet");
            }
            return View(kunde);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("Endre/{id}")]
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



        [Authorize(Roles = "Admin")]
        [HttpPost("EndreBekreftet")]
        public async Task<IActionResult> EndreBekreftet(Kunde kunde)
        {

            if (!ModelState.IsValid)
            {
                var errors = ModelState.SelectMany(x => x.Value.Errors.Select(p => new {
                    Field = x.Key,
                    Error = p.ErrorMessage
                })).ToList();

                foreach (var error in errors)
                {
                    _Brukerlogger.LogWarning("Field: {Field}, Error: {Error}", error.Field, error.Error);
                }
            }

            if (ModelState.IsValid)
            {
                bool OK = await _kundeInterface.endreKunde(kunde);
                if (OK)
                {
                    return RedirectToAction(nameof(Tabell));
                }
                else
                {
                    _Brukerlogger.LogWarning("[OrdreKontroller] oppdatering av ordre failet", kunde);
                    ModelState.AddModelError(string.Empty, "Failed to modify the Order. Please try again.");
                }
            }

            _Brukerlogger.LogWarning("[KundeController] oppdatering av kunde failet", kunde);
            return RedirectToAction($"{nameof(Tabell)}");
        }





        [HttpGet("Lag")]
        
        public IActionResult Lag()
        {

            return View();


        }


        [HttpPost("Lag")]
       
        public async Task<IActionResult> Lag(Kunde kunde)
        {
            if (ModelState.IsValid)
            {
                int id  = await _kundeInterface.Lag(kunde);
                if ( id > -1)
                {
                    return RedirectToAction(nameof(Tabell));

                }
            }
            _Brukerlogger.LogWarning("[KundeController] Kunde laging har failet", kunde);
            return View(kunde);
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("Slett/{id}")]
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

        
        [HttpDelete("SlettBekreftet")]
        public async Task<IActionResult> SlettBekreftet(int id)
        {
            bool OK = await _kundeInterface.SlettKunde(id);
            if (!OK)
            {
                _Brukerlogger.LogError("[BrukerController] sletting av bruker mislyktes for denne iden", id);
                return BadRequest("sletting av kunde failet");
            }
            return Ok();
        }
    }


  
}
