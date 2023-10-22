using Oblig1.Models;

namespace Oblig1.DAL
{
    public interface KundeInterface
    {
        Task<bool> endreKunde(Kunde kunde);
        Task<IEnumerable<Kunde>?> HentAlle();
        Task<Kunde> hentKundeMedId(int id);
        Task<int> lagKunde(Kunde kunde);
        Task<bool> SlettKunde(int id);

        Task<Kunde> finnKundeId(string id);
       
    }
}