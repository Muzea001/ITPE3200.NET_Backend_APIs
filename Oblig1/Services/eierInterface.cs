using Oblig1.Models;

namespace Oblig1.Services
{
    public interface eierInterface
    {

        Task<IEnumerable<Eier>?> HentAlle();

        Task<Eier> hentEierMedId(long id);

        Task<bool> endreEier(Eier eier);

         Task<bool> SlettEier(long id);

        Task<Eier> hentEierMedPersonId(string personId);

    }
}