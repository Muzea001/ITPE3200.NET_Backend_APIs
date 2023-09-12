using System.ComponentModel.DataAnnotations;

namespace Oblig1.Models
{
    public class Eeier
    {

       
        public Bruker bruker { get; set; }

        [RegularExpression(@"^\d{11}$")]
        public long kontoNummer { get; set; }    


    }
}
