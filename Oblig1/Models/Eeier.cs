using System.ComponentModel.DataAnnotations;

namespace Oblig1.Models
{
    public class Eeier
    {


        public virtual Bruker bruker { get; set; }

        public string brukernavn { get; set; }  

        [RegularExpression(@"^\d{11}$")]

        [Key]
        public long kontoNummer { get; set; }    


    }
}
