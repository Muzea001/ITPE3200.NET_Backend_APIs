
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oblig1.Models
{
    public class Ordre
    {
        public Ordre()
        {
            
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? ordreId { get; set; }

        [StringLength(200)]
        public string? betaltGjennom { get; set; }

        public DateTime? startDato { get; set; }

        public decimal fullPris { get; set; }

        public DateTime? sluttDato { get; set; } 

        public virtual Hus? hus { get; set; }
       
        public virtual Kunde? kunde { get; set; }

       

        

    }
}
