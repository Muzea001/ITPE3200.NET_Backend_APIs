namespace Oblig1.Models
{
    public class Kunde : Person
    {

       public  virtual Person person { get; set; }   
        public int kundeId { get; set; }

        public virtual List<Ordre> ordreListe { get; set; }




    }
}
