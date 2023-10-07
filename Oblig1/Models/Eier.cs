using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oblig1.Services;

namespace Oblig1.Models
{
    public class Eier : Person
    {
        public Eier() : base() { }

        [RegularExpression(@"^\d{11}$")]
        public long kontoNummer { get; set; }

        public virtual List<Hus> husListe { get; set; }    

        public int antallAnnonser { get; set; }


    }
}
