using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oblig1.Models
{
    public class Eier : Bruker
    {
        public Eier()
        {
        }

        public Eier(string navn, DateTime fodselsdato, string addresse, long telefonNmr, string email, string passord, Person person, bool erAdmin, bool erEier, long kontonummer) : base(navn, fodselsdato, addresse, telefonNmr, email, passord, person, erAdmin, erEier)
        {
            this.kontoNummer = kontonummer; 
        }


        public int eierID { get; set; }

        [ForeignKey("Email")]
        public string Email { get; set; }
        public virtual Bruker bruker { get; set; }

      
        [RegularExpression(@"^\d{11}$")]

        
        public long kontoNummer { get; set; }    


    }
}
