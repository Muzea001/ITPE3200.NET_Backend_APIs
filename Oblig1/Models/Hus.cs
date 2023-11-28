using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oblig1.Models
{
    public class Hus
    {

        public Hus() { }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int husId { get; set; }

        [StringLength(400)]
        public string? Beskrivelse { get; set; }

        
        public double areal { get; set; }
        
        public decimal Pris { get; set; }

        
        public string by { get; set; }
        
        public string Addresse { get; set; }

       
        public int romAntall { get; set; }  
        public virtual Kunde? kunde { get; set; }    

        public virtual Eier? eier { get; set; }

        public virtual List<Ordre?> ordreListe { get; set; }

        public virtual List<Bilder>? bildeListe { get; set; }    


        public bool harParkering { get; set; }  

        public bool erMoblert { get; set; }

       
    }
}
