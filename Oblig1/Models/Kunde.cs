namespace Oblig1.Models
{
    public class Kunde : Person
    {

       
        public int kundeId { get; set; }

        public virtual List<Ordre> ordreListe { get; set; }

    }
}
