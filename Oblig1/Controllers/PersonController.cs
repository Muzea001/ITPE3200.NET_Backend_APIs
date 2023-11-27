using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Oblig1.Models;
using Oblig1.Services;
using Oblig1.ViewModeller;

namespace Oblig1.Controllers
{
    [Route("api/Person")]
    [ApiController]
    public class PersonController : Controller
    {
   
            private readonly UserManager<Person> _userManager;
            private readonly ILogger<PersonController> _Personlogger; 
            private readonly PersonInterface _personInterface;

        public PersonController(UserManager<Person> userManager, ILogger<PersonController> personlogger, PersonInterface personInterface)
        {
            _userManager = userManager;
            _Personlogger = personlogger;
            _personInterface = personInterface;
        }

        
        [HttpGet("Tabell")]
        public async Task<IActionResult> Tabell()
            {

                var liste = await _personInterface.HentAlle();
                if (liste == null)
                {
                    _Personlogger.LogError("[OrdreController] ordre liste ikke funnet dersom _ordreRepo.HentAlle");
                    return NotFound("Ordre liste ikke funnet");
                }

                var ItemListViewModel = new ItemListViewModel(liste, "Tabell");
                return Ok(liste);
            }


        [Authorize(Roles = "Admin")]
        [HttpGet("Endre/{id}")]

            public async Task<IActionResult> Endre(string id)
            {
                var Person = await _personInterface.hentPersonMedId(id);
                if (Person == null)
                {
                    _Personlogger.LogError("[PersonController] Person ikke funnet for denne iden" + id);
                    return NotFound("Person ikke funnet");
                }
                return View(Person);
            }


        [Authorize(Roles ="Bruker")]
        [HttpGet("Hent/{id}")]

        public async Task<IActionResult> Hent(string id)
        {

            var Person = await _personInterface.hentPersonMedId(id);
            if (Person == null)
            {
                _Personlogger.LogError("[PersonController] Person ikke funnet for denne iden" + id);
                return NotFound("Person ikke funnet");
            }
            return Ok(Person);



        }

        [Authorize(Roles = "Admin")]
        [HttpPost("EndreBekreftet{id}")]
        public async Task<IActionResult> EndreBekreftet(string id, Person updatedValues)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Id is required");
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("User not found");
            }

           
            user.Navn = updatedValues.Navn;
            user.Fodselsdato = updatedValues.Fodselsdato;
            user.Addresse = updatedValues.Addresse;
            user.TelefonNmr = updatedValues.TelefonNmr;

            try
            {
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction($"{nameof(Tabell)}"); ;
                }

                // Handle other potential errors from the update operation
                var errors = result.Errors.Select(e => e.Description);
                return BadRequest(errors);
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException dbEx)
            {
                if (dbEx.InnerException is Microsoft.Data.Sqlite.SqliteException sqliteEx && sqliteEx.SqliteErrorCode == 19)
                {
                    return BadRequest("Person record utilized in another table, unable to update this record due to associations with other records.");
                }
                throw;  // If it's another exception type, rethrow it to handle it elsewhere
            }
        }












        [Authorize(Roles = "Admin")]
        [HttpGet("Slett/{id}")]

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

       
        [HttpDelete("SlettBekreftet")]
        public async Task<IActionResult> SlettBekreftet(string id)
        {
            var person = await _userManager.FindByIdAsync(id);
            if (person == null)
            {
                return NotFound("Bruker for denne id" + id + " ikke funnet");
            }
            try
            {
                var sletting = await _userManager.DeleteAsync(person);
                if (sletting.Succeeded)
                {
                    return RedirectToAction(nameof(Tabell));
                }

                var errors = sletting.Errors.Select(e => e.Description);
                return BadRequest(errors);
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException dbEx)
            {
                if(dbEx.InnerException is Microsoft.Data.Sqlite.SqliteException sqliteEx && sqliteEx.SqliteErrorCode == 19)
                {
                    return BadRequest("Person record utilized in another table, can only delete persons not associated with other records");
                }
                throw;
            }
        }
        

    }

    }


