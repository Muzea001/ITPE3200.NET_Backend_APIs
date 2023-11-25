using Microsoft.AspNetCore.Mvc;
using Oblig1.Models;

namespace Oblig1.DAL
{

    public interface OrdreInterface
    {
        Task<bool> endreOrdre(Ordre ordre);
        Task<IEnumerable<Ordre>?> HentAlle();
        Task<Ordre> hentOrdreMedId(int id);
        Task<bool> lagOrdre(Ordre ordre);
        Task<bool> SlettOrdre(int id);
        Task<IEnumerable<Ordre>?> HentMine(string email);

        Task<bool> sjekkTilgjengelighet(int husID, DateTime start, DateTime slutt);

    }
}