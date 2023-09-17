using Oblig1.Models;

namespace Oblig1.Services

{
    public interface HusInterface
    {
        Task<IEnumerable<Hus>?> hentAlle();
        Task<Hus?> hentHusMedId(int id);
        Task<bool> Lag(Hus hus);
        Task<bool> Endre(Hus hus);
        Task<bool> Slett(int id);


    }
}
