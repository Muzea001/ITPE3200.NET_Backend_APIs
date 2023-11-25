using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oblig1.Models
{
    public class Person : IdentityUser
    {

        public Person(): base() { }

        

        [Required]
        [RegularExpression(@"[0-9a-zA-ZæøåÆØÅ. \-]{2,20}", ErrorMessage = "Navnet må inneholde mellom 1 og 20 tegn")]
        public string Navn {get; set;}

        [Required]
        public DateTime Fodselsdato { get; set; }

        [Required]
        [RegularExpression(@"^[A-Za-z0-9\s\-\.,']+", ErrorMessage = "Adresse inneholder ikke tillatte symboler")]
        public string Addresse { get; set;}

        [Required]
        [RegularExpression(@"^\d{8}$")]
        public long TelefonNmr { get; set;}

        
       
        

        
        


    }
}
