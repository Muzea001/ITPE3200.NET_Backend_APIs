using Oblig1.Models;

namespace Oblig1.Services
{
    public interface BrukerInterface
    {
        Task<bool> endreBruker(Bruker bruker);
        Task<IEnumerable<Bruker>?> HentAlle();
        Task<IEnumerable<Bruker>?> HentAlleAdmins();
        Task<IEnumerable<Bruker>?> HentAlleEiere();
        Task<IEnumerable<Bruker>?> HentAlleNonAdmins();
        Task<Bruker> hentBrukerMedId(int brukerid);
        Task<bool> lagBruker(Bruker bruker);
        Task<bool> Slett(int brukerid);
        Task<bool> EndreBrukerStatus(int brukerId, bool ok);
    }
}