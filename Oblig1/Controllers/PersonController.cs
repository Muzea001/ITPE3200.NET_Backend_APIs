using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Oblig1.DAL;
using Oblig1.Models;
using Oblig1.Services;
using Oblig1.ViewModeller;

namespace Oblig1.Controllers
{
    public class PersonController : Controller
    {
   
            private readonly UserManager<Person> _userManager;
            private readonly ILogger _Personlogger;
            private readonly PersonInterface _personInterface;

        public PersonController(UserManager<Person> userManager, ILogger personlogger, PersonInterface personInterface)
        {
            _userManager = userManager;
            _Personlogger = personlogger;
            _personInterface = personInterface;
        }

        public async Task<IActionResult> Tabell()
            {

                var liste = await _personInterface.HentAlle();
                if (liste == null)
                {
                    _Personlogger.LogError("[OrdreController] ordre liste ikke funnet dersom _ordreRepo.HentAlle");
                    return NotFound("Ordre liste ikke funnet");
                }

                var ItemListViewModel = new ItemListViewModel(liste, "Tabell");
                return View(ItemListViewModel);
            }
            [HttpGet]

            public async Task<IActionResult> Endre(string id)
            {
                var Person = await _personInterface.hentPersonMedId(id);
                if (Person == null)
                {
                    _Personlogger.LogError("[OrdreKontroller] Ordre ikke funnet for denne iden" + id);
                    return NotFound("Ordre ikke funnet");
                }
                return View(Person);
            }

            [HttpPost]

            public async Task<IActionResult> EndreBekreftet(Person person)
            {
                if (ModelState.IsValid)
                {
                    bool OK = await _personInterface.endrePerson(person);   
                    if (OK)
                    {
                        return RedirectToAction(nameof(Tabell));
                    }
                    else
                    {
                        _Personlogger.LogWarning("[OrdreKontroller] oppdatering av ordre failet", person);

                        ModelState.AddModelError(string.Empty, "Failed to modify the Order. Please try again.");
                    }

                }
                else
                {
                    _Personlogger.LogWarning("[OrdreKontroller] Invalid model state.", person);

                    foreach (var modelStateKey in ViewData.ModelState.Keys)
                    {
                        var modelStateVal = ViewData.ModelState[modelStateKey];
                        foreach (var error in modelStateVal.Errors)
                        {

                            _Personlogger.LogWarning($"Key: {modelStateKey}, Error: {error.ErrorMessage}");
                        }
                    }
                }


                return RedirectToAction($"{nameof(Tabell)}");




            }

 

          

            [HttpGet]

            public async Task<IActionResult> Slett(string id)
            {
                var person = await _personInterface.hentPersonMedId(id);
                if (person == null)
                {
                    _Personlogger.LogError("[OrdreKontroller] Ordre ikke funnet for denne iden", id);
                    return BadRequest("Ordre ikke funnet for id gitt");

                }
                return View(person);

            }

            [HttpPost]

            public async Task<IActionResult> SlettBekreftet(string id )
            {
                bool OK = await _personInterface.SlettPerson(id);
                if (!OK)
                {
                    _Personlogger.LogError("[OrdreKontroller] sletting av bruker mislyktes for denne iden", id);
                    return BadRequest("sletting av ordre failet");
                }
                return RedirectToAction(nameof(Tabell));
            }


        }

    }


