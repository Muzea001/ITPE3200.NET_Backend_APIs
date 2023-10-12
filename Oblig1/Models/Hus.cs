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

        [Range(0.01,double.MaxValue, ErrorMessage ="areal må være større enn null")]
        public double areal { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage = "pris må være større enn null")]
        public decimal Pris { get; set; }

        [RegularExpression(@"^[A-Za-z\s\-\.,']+", ErrorMessage = "by inneholder ikke tillatte symboler eller tall")]
        public string by { get; set; }
        [RegularExpression(@"^[A-Za-z0-9\s\-\.,']+", ErrorMessage = "Adresse inneholder ikke tillatte symboler")]
        public string Addresse { get; set; }
        [Range(1, 20, ErrorMessage = "antall rom må være 0-20")]
        public int romAntall { get; set; }  

        public bool erTilgjengelig { get; set; }

        public virtual Kunde? kunde { get; set; }    

        public virtual Eier eier { get; set; }

        public virtual List<Ordre> ordreListe { get; set; }

        public string bildeURL { get; set; }    

        public bool harParkering { get; set; }  

        public bool erMoblert { get; set; }

       
    }
}
