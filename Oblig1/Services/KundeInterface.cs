using Oblig1.Models;

namespace Oblig1.DAL
{
    public interface KundeInterface
    {
        Task<bool> endreKunde(Kunde kunde);
        Task<IEnumerable<Kunde>?> HentAlle();
        Task<Kunde> hentKundeMedId(int id);
        Task<bool> lagKunde(Kunde kunde);
        Task<bool> SlettKunde(int id);
    }
}