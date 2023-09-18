using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oblig1.Models
{
    public class Person
    {

        public Person() { }


        public Person(string navn, DateTime fodselsdato, string addresse, long telefonNmr, string email)
        {
            Navn = navn;
            Fodselsdato = fodselsdato;
            Addresse = addresse;
            TelefonNmr = telefonNmr;
            Email = email;
        }

        public int personID { get; set; }

        [RegularExpression(@"[0-9a-zA-ZæøåÆØÅ. \-]{2,20}", ErrorMessage = "Navnet må inneholde mellom 1 og 20 tegn")]
        public string Navn {get; set;}

   
        public DateTime Fodselsdato { get; set; }

        [RegularExpression(@"^[A-Za-z0-9\s\-\.,']+", ErrorMessage = "Adresse inneholder ikke tillatte symboler")]
        public string Addresse { get; set;}

        [RegularExpression(@"^\d{8}$")]
        public long TelefonNmr { get; set;}

        
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")]
        [ForeignKey("Email")]
        public virtual string Email { get; set;}


    }
}
