using Oblig1.Models;

namespace Oblig1.ViewModeller
{
    public class ItemListViewModel
    {
        
        public IEnumerable<Hus> Hus;
        public IEnumerable<Ordre> Ordre;
        public IEnumerable<Eier> Eier;
        public IEnumerable<Kunde> Kunde;
        public IEnumerable<Person> Person;
        public string? CurrentViewName;
       

        public ItemListViewModel(IEnumerable<Hus> hus, string? viewNavn)
        {
            Hus = hus;
            CurrentViewName = viewNavn;
        }

        public ItemListViewModel(IEnumerable<Kunde> kunde, string? viewNavn)
        {
            Kunde = kunde;
            CurrentViewName = viewNavn;
        }

        public ItemListViewModel(IEnumerable<Eier> eier, string? viewNavn)
        {
            Eier = eier;
            CurrentViewName = viewNavn;
        }

        public ItemListViewModel(IEnumerable<Person> person, string? viewNavn)
        {
            Person = person;
            CurrentViewName = viewNavn;
        }

        public ItemListViewModel (IEnumerable<Ordre> ordre, string? viewNavn){

            Ordre = ordre;
            CurrentViewName = viewNavn;

        }
       

        public ItemListViewModel(Hus liste, string v)
        {
        }
    }
}
