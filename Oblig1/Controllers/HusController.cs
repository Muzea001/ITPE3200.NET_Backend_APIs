using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Oblig1.DAL;
using Oblig1.Models;
using Oblig1.Services;
using Oblig1.ViewModeller;
using System.Drawing;

namespace Oblig1.Controllers
{
    public class HusController : Controller
    {
        private readonly ILogger _HusLogger;

        private readonly HusInterface husInterface;



        public HusController(HusInterface Interface, ILogger<HusController> logger)
        {
            husInterface = Interface;
            _HusLogger = logger;


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

        [HttpPost]
        
        public async Task<IActionResult> Create(Hus hus, IFormFile imageData)
        {
            if (ModelState.IsValid)
            {
                bool OK = await husInterface.Lag(hus);
                if (OK)
                {
                    if (imageData!= null && imageData.Length> 0)
                    {
                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Bilder");
                        var uniqueFileName = Guid.NewGuid().ToString() + "_ " + imageData.FileName;
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageData.CopyToAsync(stream);
                        }

                    }
                    return RedirectToAction(nameof(Tabell));

                }
            }
            _HusLogger.LogWarning("[HusController] Hus laging har failet", hus);
            return View(hus);
        }


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

