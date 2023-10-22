using Oblig1.Models;

namespace Oblig1.Controllers
{
    public class MyViewModel
    {
        public Person Person { get; set; }
        public Hus hus { get; set; }

        public Kunde kunde { get; set; }    
        public Ordre ordre { get; set; }
    }
}
