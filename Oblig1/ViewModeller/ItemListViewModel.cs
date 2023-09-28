using Oblig1.Models;

namespace Oblig1.ViewModeller
{
    public class ItemListViewModel
    {
        public IEnumerable<Hus> Hus;
        public IEnumerable<Ordre> Ordre;
        public IEnumerable<Bruker> Bruker;
        public IEnumerable<Kunde> Kunde;
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

        public ItemListViewModel(IEnumerable<Bruker> bruker, string? viewNavn)
        {
            Bruker = bruker;
            CurrentViewName = viewNavn;
        }

        public ItemListViewModel (IEnumerable<Ordre> ordre, string? viewNavn){

            Ordre = ordre;
            CurrentViewName = viewNavn;

        }

        
    }
}
