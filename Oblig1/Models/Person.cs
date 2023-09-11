using System.ComponentModel.DataAnnotations;

namespace Oblig1.Models
{
    public class Person
    {
        public int Id { get; set; }

        [RegularExpression(@"[0-9a-zA-ZæøåÆØÅ. \-]{2,20}", ErrorMessage = "Navnet må inneholde mellom 1 og 20 tegn")]
        public string Navn {get; set;}


        private DateTime dato;
        public DateTime Fodselsdato {

            get { return dato; }
            set
            {

                DateTime voksen = DateTime.Now.AddYears(-18);

                if (value > voksen)
                {
                    throw new ArgumentException("Person må være eldre enn 18 !");
                }

                dato = value;
            }
                }

        [RegularExpression(@"^[A-Za-z0-9\s\-\.,']+", ErrorMessage = "Adresse inneholder ikke tillatte symboler")]
        public string Addresse { get; set;}

        [RegularExpression(@"^\d{8}$")]
        public int TelefonNmr { get; set;}
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")]
        public string Email { get; set;}


    }
}
