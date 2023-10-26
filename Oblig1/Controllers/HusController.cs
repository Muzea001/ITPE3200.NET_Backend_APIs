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



        public HusController(HusInterface Interface, ILogger<HusController> logger, UserManager<Person> userManager, ItemDbContext itemDbContext)
        {
            husInterface = Interface;
            _HusLogger = logger;
            _userManager= userManager;
            _db = itemDbContext;
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
        [ValidateAntiForgeryToken] // For security purposes
        public async Task<IActionResult> Create(Hus hus, List<IFormFile> bildeListe)
        {
            if (ModelState.IsValid) // Check if the model's state is valid based on your annotations
            {
                try
                {
                    
                    var imageUrls = new List<string>();
                    foreach (var image in bildeListe)
                    {
                        var fileName = Path.GetFileName(image.FileName);
                        var filePath = Path.Combine("wwwroot/Bilder", fileName); // Adjust this path as necessary
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await image.CopyToAsync(stream);
                        }
                        imageUrls.Add("/Bilder/" + fileName); // Adjust this path as necessary
                    }

                    // Assign the image URLs to the house object
                    hus.bildeListe = imageUrls.Select(url => new Bilder { bilderUrl = url }).ToList();

                    // Assuming your logged-in user's kontoNummer can be fetched this way
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    var eier = _db.Eier.FirstOrDefault(e => e.Person.Id == userId); // Assuming you have a _dbContext with your EF context

                    if (eier != null)
                    {
                        hus.eier = eier; // Set the house's owner to the current user

                        // Now, save the house to the database using your interface
                        await husInterface.Lag(hus);

                        // If all goes well, redirect to the Kvittering view with the house object
                        return RedirectToAction("Kvittering", hus);
                    }
                    else
                    {
                        // Handle the case where the current user is not an Eier or other issues
                        ModelState.AddModelError("", "Current user is not a valid house owner.");
                    }
                }
                catch (Exception ex)
                {
                    // Handle the exception appropriately (log it, etc.)
                    ModelState.AddModelError("", "An error occurred while processing your request.");
                }
            }

            // If we get here, something went wrong. Display the error to the user.
            return View("Error"); // You should have a view named "Error" with a proper message to the user.
        }








        public async Task<IActionResult> Kvittering(int id)
        {
            var hus = await husInterface.hentHusMedId(id);
            if (hus == null)
            {
                return NotFound("The specified house could not be found.");
                _HusLogger.LogError($"No Hus found with ID: {id}");
            }

            
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

