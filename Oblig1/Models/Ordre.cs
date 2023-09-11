using Castle.Components.DictionaryAdapter;
using System.ComponentModel.DataAnnotations;

namespace Oblig1.Models
{
    public class Ordre
    {

        public int ordreId { get; set; }

        public DateTime Dato { get; set; }

        [StringLength(200)]
        public string betaltGjennom { get; set; }   
        public virtual Hus hus { get; set; }    

        public int husId { get; set; }

       




    }
}
