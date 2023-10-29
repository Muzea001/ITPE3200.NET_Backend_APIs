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
    public class HusController : Controller
    {
        private readonly ItemDbContext _db;

        private readonly ILogger _HusLogger;

        private readonly HusInterface husInterface;

        private readonly UserManager<Person> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly eierInterface _eierInterface;

        public HusController(HusInterface Interface, ILogger<HusController> logger, UserManager<Person> userManager, ItemDbContext itemDbContext, IWebHostEnvironment webHostEnvironment, eierInterface eierInterface)
        {
            husInterface = Interface;
            _HusLogger = logger;
            _userManager = userManager;
            _db = itemDbContext;
            _webHostEnvironment = webHostEnvironment;
            _eierInterface = eierInterface;
        }

        public async Task<IActionResult> Tabell()
        {
            var liste = await husInterface.hentAlle();
            if (liste == null)
            {
                _HusLogger.LogError("[HusController] hus liste ikke funnet dersom hentAlle() ble kalt");
                return NotFound("hus liste ikke funnet");

            }


            var itemListViewModel = new ItemListViewModel(liste, "Tabell");
            return View(itemListViewModel);

        }

        public async Task<IActionResult> tilgjengeligHus()
        {
            var liste = await husInterface.hentAlle();
            if (liste == null)
            {
                _HusLogger.LogError("[HusController] hus liste ikke funnet dersom hentAlle() ble kalt");
                return NotFound("hus liste ikke funnet");

            }


            var itemListViewModel = new ItemListViewModel(liste, "Grid");
            return View(itemListViewModel);

        }

        public async Task<IActionResult> hentMedFilter(string by, int minstAreal, int maksAreal, int minPris, int maksPris, int minstRom, int maksRom)
        {
            var liste = await husInterface.hentAlleMedFilter(by, minstAreal, maksAreal, minPris, maksPris, minstRom, maksRom);
            if (liste == null)
            {
                return NotFound("ingenting funnet");
            }

            var itemListViewModel = new ItemListViewModel(liste, "Tabell");
            return View(itemListViewModel);

        }

        [HttpGet]
        public async Task<IActionResult> Oversikt(int id)
        {
            var hus = await husInterface.hentHusMedId(id);
            if (hus == null)
            {
                _HusLogger.LogError("[HusController] hus var ikke funnet ved bruk av denne iden : " + id);
                return NotFound("hus var ikke funnet");
            }

            return View(hus);


        }



        
        [HttpGet]
        public IActionResult Create()
        {

            return View();


        }



        [Authorize(Roles = "Admin, Bruker")]
        [HttpPost]
        public async Task<IActionResult> CreateHouseWithImages(HusOgBilderViewModell viewModel)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var personID = _userManager.GetUserId(User);
                    var person = await _userManager.FindByIdAsync(personID);
                    var eier = await _db.Eier.FirstOrDefaultAsync(e => e.Person.Id == personID);
                    if (eier == null)
                    {
                       
                        eier = new Eier { Person = person, husListe = new List<Hus>(), antallAnnonser = 0 };
                        _db.Eier.Add(eier); 
                    }
                    viewModel.hus.eier = eier;
                    
                    
                    bool OK = await husInterface.Lag(viewModel.hus);
                    if (OK)
                    {
                        viewModel.hus.eier.husListe.Add(viewModel.hus);
                        viewModel.hus.eier.antallAnnonser++;
                    }
                    string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "Bilder");
                    foreach (var file in viewModel.bilder)
                    {
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;

                        var filePath = Path.Combine(uploadPath, uniqueFileName);

                        // Save the file to the Bilder directory.
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }

                        // Create a new Bilder instance.
                        var newBilde = new Bilder
                        {
                            bilderUrl = "/Bilder/" + uniqueFileName, 
                            Hus = viewModel.hus 
                        };

                        _db.Bilder.Add(newBilde);
                    }

                    await _db.SaveChangesAsync();
                    transaction.Commit();

                    return RedirectToAction("Index","Home");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    
                    return View("ErrorView", ex);
                }
            }
        }






        [Authorize(Roles = "Admin, Bruker")]
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
        [HttpGet]
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
        [HttpPost]
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

            // If you want to display the error on the same page, consider returning View with the model
            // return View(hus);

            return RedirectToAction(nameof(Tabell));
        }

        [HttpGet]
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

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SlettBekreftet(int id, int eierId)
        {


            bool OK = await husInterface.Slett(id);
            bool OKEier = await husInterface.SlettEier(eierId);
            if (!OK)
            {
                _HusLogger.LogWarning("[HusController] sletting av bruker failet" + id);
                return BadRequest("Sletting av hus mislyktes");
            }
            return RedirectToAction($"{nameof(Tabell)}");

        }





    }
}

