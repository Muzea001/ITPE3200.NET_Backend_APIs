using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Oblig1.DAL;
using Oblig1.Models;
using System.Text.Encodings.Web;
using System.Text;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Oblig1.Services;

namespace Oblig1.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class BrukerController : ControllerBase
    {
        private readonly ItemDbContext _itemDbContext;
        private readonly UserManager<Person> _userManager;
        private readonly SignInManager<Person> _signInManager;
        private readonly ILogger<BrukerController> _logger;
        private readonly IEmailSender _emailSender;
        private readonly PersonInterface _personInterface;

        public BrukerController(UserManager<Person> userManager, SignInManager<Person> signInManager, ILogger<BrukerController> logger, ItemDbContext itemDbContext , IEmailSender emailSender, PersonInterface personInterface)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _itemDbContext = itemDbContext;
            _emailSender = emailSender; 
            _personInterface = personInterface;
        }



        [HttpGet("brukerInfo/{id}")]
        
        public async Task<IActionResult> hentBrukerInfo(string id)
        {
            try
            {
                
                var bruker = await _personInterface.hentPersonMedId(id);

                if (bruker != null)
                {
                    
                    var userInfo = new
                    {
                        brukerId = bruker.Id,
                        email = bruker.Email,
                        navn = bruker.Navn,
                        telefonNummer = bruker.TelefonNmr,
                        addresse = bruker.Addresse,
                        fodselsdato = bruker.Fodselsdato
                        
                    };

                    return Ok(userInfo);
                }
                else
                {
                    return NotFound("Bruker Ikke Funnet");
                }
            }
            catch (Exception ex)
            {
                
                _logger.LogError(ex, "Error while retrieving user information");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
            


        



        public class RegisterModel
        {
            public string Navn { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }

            public int Telefonnummer { get; set; }

            public DateTime Fodselsdato { get; set; }

            public string Addresse { get; set; }

            public string BekreftPassword { get; set; }

            
        }

        [HttpPost("Registrer")]
       
        public async Task<IActionResult> Registrer([FromBody] RegisterModel model)
        {

            if (!ModelState.IsValid)
            {
                foreach (var modelStateKey in ModelState.Keys)
                {
                    var modelStateVal = ModelState[modelStateKey];
                    foreach (var error in modelStateVal.Errors)
                    {
                       
                        _logger.LogError($"{modelStateKey}: {error.ErrorMessage}");
                    }
                }

                return BadRequest(ModelState); 
            }

            if (ModelState.IsValid)
            {

                var person = new Person
                {
                    Navn = model.Navn,
                    UserName = model.Email,
                    Email = model.Email,
                    Fodselsdato = model.Fodselsdato,
                    TelefonNmr = model.Telefonnummer,
                    Addresse = model.Addresse



                };



                var result = await _userManager.CreateAsync(person, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(person, "Bruker");
                    await _itemDbContext.SaveChangesAsync();
                    _logger.LogInformation("User created a new account with password.");

                    var userId = await _userManager.GetUserIdAsync(person);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(person);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = Url.Action(
                    "ConfirmEmail", // Action Name
                    "Account",      // Controller Name
                    new { userId = userId, code = code }, // Route Values
                    protocol: HttpContext.Request.Scheme);

                    await _emailSender.SendEmailAsync(model.Email, "Confirm your email",
                    $"Please confirm your account by clicking this link: {callbackUrl}");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return Ok(new { Id = userId, email = model.Email, status = "Registration successful, email confirmation required." });
                    }
                
                    else
                    {
                    return Ok(new { Id = userId, email = model.Email, status = "Registration successful." });
                }
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }

            
            return BadRequest(ModelState);






        }

        public class LogInnModel
        {

            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            public bool RememberMe { get; set; }


        }


        [HttpPost("LoggUt")]
        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                // This needs to be a redirect so that the browser performs a new
                // request and the identity for the user gets updated.
                return Ok(new {message = "Logged Out Successfully" ! });
            }
        }

        [HttpPost("LoggInn")]

        public async Task<IActionResult> LogInn([FromBody] LogInnModel loginModel)
        {
            

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginModel.Email, loginModel.Password, loginModel.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return Ok(new { Message = "Login Successful" });
                }
                if (result.RequiresTwoFactor)
                {
                   
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                   
                    return StatusCode(StatusCodes.Status403Forbidden, new { Message = "User account is locked out." });
                }
                else
                {
                    return BadRequest("Invalid login attempt.");
                }
            }

           
            return BadRequest(ModelState);
        }







    }
}
