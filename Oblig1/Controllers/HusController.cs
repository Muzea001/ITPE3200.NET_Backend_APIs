using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Oblig1.DAL;
using Oblig1.Models;
using Oblig1.Services;
using Oblig1.ViewModeller;
using System;
using System.Drawing;
using System.Linq.Expressions;
using System.Security.Claims;

namespace Oblig1.Controllers
{

    [Route("api/Hus")]
    [ApiController]
    public class HusController : Controller
    {
        private readonly ItemDbContext _db;

        private readonly ILogger _HusLogger;

        private readonly HusInterface husInterface;

        private readonly UserManager<Person> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly PersonInterface _personInterface;
        private readonly eierInterface _eierInterface;

        public HusController(HusInterface Interface, ILogger<HusController> logger, UserManager<Person> userManager,
            ItemDbContext itemDbContext, IWebHostEnvironment webHostEnvironment, PersonInterface personInterface, eierInterface eierInterface)
        {
            husInterface = Interface;
            _HusLogger = logger;
            _userManager = userManager;
            _db = itemDbContext;
            _webHostEnvironment = webHostEnvironment;
            _eierInterface = eierInterface;
            _personInterface = personInterface;
        }

        [HttpGet("Tabell")]
        public async Task<IActionResult> Tabell()
        {
            var liste = await husInterface.hentAlle();
            if (liste == null)
            {
                _HusLogger.LogError("[HusController] hus liste ikke funnet dersom hentAlle() ble kalt");
                return NotFound("hus liste ikke funnet");

            }



            return Ok(liste);

        }


        [HttpGet("HentMine/{email}")]
        public async Task<IActionResult> HentMine(string email)
        {

            var Hus = await husInterface.hentMine(email);
            if (Hus == null)
            {
                _HusLogger.LogError("[OrdreController] ordre liste ikke funnet for denne iden" + email);
                return NotFound();
            }

            return Ok(Hus);



        }




        [HttpGet("Oversikt/{id}")]
        public async Task<IActionResult> Oversikt(int id)
        {
            var hus = await husInterface.hentHusMedId(id);
            if (hus == null)
            {
                _HusLogger.LogError("[HusController] hus var ikke funnet ved bruk av denne iden : " + id);
                return NotFound("hus var ikke funnet");
            }

            return Ok(hus);


        }




        [HttpGet("Create")]
        public IActionResult Create()
        {

            return View();


        }




        [HttpPost("CreateHouseWithImages")]
        public async Task<IActionResult> CreateHouseWithImages([FromForm] IEnumerable<IFormFile> bilder, [FromForm] Hus hus,[FromForm] long kontonummer, [FromQuery] string email)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                try
                {
                    var person = await _personInterface.hentPersonMedEmail(email);
                    if (person == null)
                    {
                        return NotFound();
                    }



                    Eier eier = await _eierInterface.hentEierMedId(kontonummer);
                    if (eier == null)
                    {
                        eier = new Eier { Person = person, kontoNummer = kontonummer, antallAnnonser = 0 };
                    }
                        


                    hus.bildeListe = new List<Bilder>();
                    hus.eier = eier;
               
                    string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "Bilder");
                    foreach (var file in bilder)
                    {
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;

                        var filePath = Path.Combine(uploadPath, uniqueFileName);

                        // Save the file to the Bilder directory.
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }


                        var newBilde = new Bilder
                        {
                            bilderUrl = "/Bilder/" + uniqueFileName,
                            Hus = hus
                        };
                        hus.bildeListe.Add(newBilde);
                        _db.Bilder.Add(newBilde);
                    }
                    bool OK = await husInterface.Lag(hus);
                    if (OK)
                    {
                        eier.husListe.Add(hus);
                        eier.antallAnnonser++;
                    }
                    await _db.SaveChangesAsync();
                    transaction.Commit();

                    return Ok(new { message = "House created successfully", houseId = hus.husId });
                }


                catch (Exception ex)
                {
                    transaction.Rollback();
                    _HusLogger.LogError(ex, "Error occurred in CreateHouseWithImages");
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred: " + ex.ToString() });
                }
            }
        }
    
            
        






        [Authorize(Roles = "Admin, Bruker")]
        [HttpGet("Kvittering/{id}")]
        public async Task<IActionResult> Kvittering(int id)
        {
            var hus = _db.Hus.Include(h => h.bildeListe).FirstOrDefault(h => h.husId == id);

            if (hus == null)
            {
                return NotFound("The specified house could not be found.");
                _HusLogger.LogError($"No Hus found with ID: {id}");
            }

            
            return View(hus);
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> endreHus(int id)
        {
            var hus = await husInterface.hentHusMedId(id);
            if (hus == null)
            {
                _HusLogger.LogError("[HusController] hus ikke funnet ved bruke av dette brukernavnet : " + id);
                return NotFound("hus ikke funnet");
            }
            return View(hus);

        }

        [Authorize(Roles = "Admin")]
        [HttpPost("EndreBekreftet")]
        public async Task<IActionResult> EndreBekreftet(Hus hus)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool OK = await husInterface.Endre(hus);
                    if (OK)
                    {
                        return RedirectToAction(nameof(Tabell));
                    }
                    else
                    {
                        _HusLogger.LogWarning("[HusController] Failed to modify the house. House ID: {houseId}", hus.husId);
                        ModelState.AddModelError(string.Empty, "Failed to modify the house. Please try again.");
                    }
                }
                catch (Exception ex)
                {
                    _HusLogger.LogError(ex, "[HusController] Exception occurred while modifying the house. House ID: {houseId}", hus.husId);
                }
            }
            else
            {
                _HusLogger.LogWarning("[HusController] Invalid model state for House ID: {houseId}. Errors:", hus.husId);
                foreach (var modelStateKey in ViewData.ModelState.Keys)
                {
                    var modelStateVal = ViewData.ModelState[modelStateKey];
                    foreach (var error in modelStateVal.Errors)
                    {
                        _HusLogger.LogWarning("Key: {key}, Error: {error}", modelStateKey, error.ErrorMessage);
                    }
                }
            }

           

            return RedirectToAction(nameof(Tabell));
        }

        [HttpGet("Slett/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Slett(int id)
        {
            var hus = await husInterface.hentHusMedId(id);
            if (hus == null)
            {
                _HusLogger.LogError("[HusController] bruker ikke funnet ved bruke av dette brukernavnet : " + id);
                return NotFound("hus ikke funnet");
            }
            return View(hus);

        }

        [HttpDelete("SlettBekreftet")]
        
        public async Task<IActionResult> SlettBekreftet(int id)
        {


            bool OK = await husInterface.Slett(id);
           
            if (!OK)
            {
                _HusLogger.LogWarning("[HusController] sletting av bruker failet" + id);
                return BadRequest("Sletting av hus mislyktes");
            }
            return Ok();

        }





    }
}

