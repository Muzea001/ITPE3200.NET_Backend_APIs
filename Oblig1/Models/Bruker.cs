using System.ComponentModel.DataAnnotations;

namespace Oblig1.Models
{
    public class Bruker : Person

    {
        
        public Bruker(string navn,DateTime fodselsdato, string addresse, int telefonnummer, string email,string passord, bool erEier, bool erAdmin)
        :base(navn,fodselsdato,addresse,telefonnummer,email)
        {
            this.Passord = passord;
            this.erEier = erEier;
            this.erAdmin = erAdmin;
        
        
        
        }

        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z]).{8,}$")]
        String Passord { get; set; }
        bool erAdmin { get; set; }

        bool erEier { get; set; }
    }
}
