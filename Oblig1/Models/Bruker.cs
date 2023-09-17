using System.ComponentModel.DataAnnotations;

namespace Oblig1.Models
{
    public class Bruker : Person

    {
        public Bruker(string navn, DateTime fodselsdato, string addresse, int telefonNmr, string email) : base(navn, fodselsdato, addresse, telefonNmr, email)
        {
        }

        public virtual Person person { get; set; }  

        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z]).{8,}$")]
        String Passord { get; set; }
        bool erAdmin { get; set; }

        bool erEier { get; set; }
    }
}
