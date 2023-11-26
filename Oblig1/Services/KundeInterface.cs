using Microsoft.AspNetCore.Mvc;
using Oblig1.Models;

namespace Oblig1.DAL
{
    public interface KundeInterface
    {
        Task<bool> endreKunde(Kunde kunde);
        Task<IEnumerable<Kunde>?> HentAlle();
        Task<Kunde> hentKundeMedId(int id);
      
        Task<bool> SlettKunde(int id);

        Task<Kunde> hentKundeId(string personId);

        Task<int> Lag(Kunde kunde);

        Task<Kunde> hentKundeMedPersonId(string personId);
    }
}