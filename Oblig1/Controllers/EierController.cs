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
    [Route("api/Eier")]
    [ApiController]

    public class EierController : Controller
    {

        private readonly UserManager<Person> _userManager;
        private readonly ILogger<Eier> _EierLogger;
        private readonly eierInterface _eierInterface;

        public EierController(UserManager<Person> userManager, ILogger<Eier> eierlogger, eierInterface eierInterface)
        {
            _userManager = userManager;
            _EierLogger = eierlogger;
            _eierInterface = eierInterface;
        }

        [HttpGet("Tabell")]
        
        public async Task<IActionResult> Tabell()
        {
            var liste = await _eierInterface.HentAlle();
            if (liste == null)
            {
                _EierLogger.LogError("[EierLogger] eier liste ikke funnet dersom _ordreRepo.HentAlle");
                return NotFound("Ordre liste ikke funnet");
            }

            var ItemListViewModel = new ItemListViewModel(liste, "Tabell");
            return Ok(liste);
        }

        [HttpGet("hentMin/{id}")]
        public async Task<IActionResult> hentMin(string id)
        {

            var eier = await _eierInterface.hentEierMedPersonId(id);
            if (eier == null)
            {
                _EierLogger.LogError("[EierKontroller] Eier ikke funnet for denne iden" + id);
                return NotFound("Eier ikke funnet");
            }
            return View(eier);
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("Endre/{id}")]

        public async Task<IActionResult> Endre(long id)
        {
            var eier = await _eierInterface.hentEierMedId(id);
            if (eier == null)
            {
                _EierLogger.LogError("[EierKontroller] eier ikke funnet for denne iden" + id);
                return NotFound("eier ikke funnet");
            }
            return View(eier);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("EndreBekreftet")]

        public async Task<IActionResult> EndreBekreftet(Eier eier)
        {
            if (ModelState.IsValid)
            {
                bool OK = await _eierInterface.endreEier(eier);
                if (OK)
                {
                    return RedirectToAction(nameof(Tabell));
                }
                else
                {
                    _EierLogger.LogWarning("[EierController] oppdatering av eier failet", eier);

                    ModelState.AddModelError(string.Empty, "Failed to modify the Owner. Please try again.");
                }

            }
            else
            {
                _EierLogger.LogWarning("[EierController] Invalid model state.", eier);

                foreach (var modelStateKey in ViewData.ModelState.Keys)
                {
                    var modelStateVal = ViewData.ModelState[modelStateKey];
                    foreach (var error in modelStateVal.Errors)
                    {

                        _EierLogger.LogWarning($"Key: {modelStateKey}, Error: {error.ErrorMessage}");
                    }
                }
            }
            return RedirectToAction($"{nameof(Tabell)}");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("Slett/{id}")]
        public async Task<IActionResult> Slett(long id)
        {
            var eier = await _eierInterface.hentEierMedId(id);
            if (eier == null)
            {
                _EierLogger.LogError("[EierContorller] Ordre ikke funnet for denne iden", id);
                return BadRequest("Eier ikke funnet for id gitt");

            }
            return View(eier);

        }

        
        [HttpDelete("SlettBekreftet")]

        public async Task<IActionResult> SlettBekreftet(long id)
        {
            bool OK = await _eierInterface.SlettEier(id);
            if (!OK)
            {
                _EierLogger.LogError("[EierController] sletting av bruker mislyktes for denne iden", id);
                return BadRequest("sletting av eier failet");
            }
            return Ok();
        }


    }
}
    

