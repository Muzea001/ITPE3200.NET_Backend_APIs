using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Oblig1.Models
{
    public class Kunde 
    {
        public Kunde() { }

        [NotNull]
        public virtual Person Person { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int kundeID { get; set; }    

        public virtual List<Ordre>? ordreListe { get; set; }

        public virtual List<Hus>? husListe { get; set; }




    }
}
