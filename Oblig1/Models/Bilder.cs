using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oblig1.Models
{
    public class Bilder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int bilderId { get; set; }   

        public string bilderUrl { get; set;}

        public virtual Hus? Hus { get; set; }
       


    }
}
