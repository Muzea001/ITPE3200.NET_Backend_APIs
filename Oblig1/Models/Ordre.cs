using Castle.Components.DictionaryAdapter;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oblig1.Models
{
    public class Ordre
    {
        public Ordre()
        {
            
        }

        public int ordreId { get; set; }

        public DateTime Dato { get; set; }

        [StringLength(200)]
        public string betaltGjennom { get; set; }

        [ForeignKey("husID")]
        public int husID { get; set; }  
        public virtual Hus hus { get; set; }
        [ForeignKey("eierID")]
        public int kundeID { get; set; }    

        public virtual Kunde kunde { get; set; }

        



       




    }
}
