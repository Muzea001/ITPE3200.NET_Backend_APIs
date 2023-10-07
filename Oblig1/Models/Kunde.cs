using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oblig1.Models
{
    public class Kunde : Person
    {
        public Kunde() : base() { } 
        
        public virtual List<Ordre> ordreListe { get; set; }




    }
}
