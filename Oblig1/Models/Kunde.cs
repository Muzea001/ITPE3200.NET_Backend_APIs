using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oblig1.Models
{
    public class Kunde
    {

        public Kunde() { }
       
        public int kundeId { get; set; }

        public  virtual Person Person { get; set; }

        public virtual List<Ordre> ordreListe { get; set; }




    }
}
