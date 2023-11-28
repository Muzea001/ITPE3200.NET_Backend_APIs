using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Oblig1.Services;

namespace Oblig1.Models
{
    public class Eier 
    {
        public Eier(){ }

        [RegularExpression(@"^\d{11}$")]
        [Key]
        public long kontoNummer { get; set; }

        public virtual List<Hus>? husListe { get; set; }


        [NotNull]
        public virtual Person Person { get; set; }

        public int antallAnnonser { get; set; }


    }
}
