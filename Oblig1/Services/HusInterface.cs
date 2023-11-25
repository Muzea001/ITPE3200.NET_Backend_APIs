using Oblig1.Models;

namespace Oblig1.Services

{
    public interface HusInterface
    {
        Task<IEnumerable<Hus>> hentAlle();
        Task<Hus> hentHusMedId(int id);

        Task<IEnumerable<Hus>> hentMine(string email);
        Task<Hus> hentAlleMedFilter(string by, int minstAreal, int maksAreal, int minPris, int maksPris, int minstRom, int maksRom);
        Task<bool> Lag(Hus hus);
        Task<bool> Endre(Hus hus);
        Task<bool> Slett(int id);

        Task<bool> SlettEier(int id);

    }
}
