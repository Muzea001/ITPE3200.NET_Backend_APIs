using System.ComponentModel.DataAnnotations;

namespace Oblig1.Models
{
    public class Bruker : Person
    {

        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z]).{8,}$")]
        String Passord { get; set; }
        bool erAdmin { get; set; }

        bool erEier { get; set; }
    }
}
