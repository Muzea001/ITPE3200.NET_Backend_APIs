using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oblig1.Models
{
    public class Bruker : Person

    {

        public Bruker() : base() { }

        public Bruker(string navn, DateTime fodselsdato, string addresse, long telefonNmr, string email,string passord, Person person, bool erAdmin, bool erEier ) : base(navn, fodselsdato, addresse, telefonNmr, email)
        {

            this.Passord = passord; 
            this.erAdmin = erAdmin; 
            this.erEier = erEier;
          
        }
        public int personID { get; set; }
        [ForeignKey("personID")]
        public virtual Person person { get; set; }

        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z]).{8,}$")]
        public String Passord { get; set; }
        public bool erAdmin { get; set; }

        public bool erEier { get; set; }
    }
}
