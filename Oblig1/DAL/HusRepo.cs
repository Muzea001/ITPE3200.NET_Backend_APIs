using Microsoft.EntityFrameworkCore;
using Oblig1.Models;
using Oblig1.Services;
using System.Linq.Expressions;

namespace Oblig1.DAL
{
    public class HusRepo : HusInterface
    {
        private readonly ItemDbContext _db;

        private readonly ILogger<HusRepo> _logger;

        public HusRepo(ItemDbContext db, ILogger<HusRepo> logger)
        {
            _db = db;
            _logger = logger;
        }


        public async Task<IEnumerable<Hus>> hentAlle()
        {
            try
            {
                return await _db.hus.Include(h => h.bildeListe).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("[HusRepo] hus ToListAsync failet når hentalle ble kallet, error melding : {e}", ex.Message);
                return null;
            }

        }


        public async Task<IEnumerable<Hus>> HentAlleTilgjengelig()
        {
            try
            {
                return await _db.hus.FromSqlRaw("SELECT* FROM hus WHERE erTilgjengelig=1").ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("[HusRepo] hus ToListAsync failet når hentalle ble kallet, error melding : {e}", ex.Message);
                return null;
            }

        }

        public async Task<IEnumerable<Hus>> hentAlleFilter(string by, int minstAreal, int maksAreal, int minPris, int maksPris, int minstRom, int maksRom)
        {
            try
            {
                var sporring = _db.hus.AsQueryable();

                if (!string.IsNullOrEmpty(by))
                {
                    sporring = sporring.Where(h => EF.Functions.Like(h.by, $"%{by}%"));
                }

                if (minPris > 0)
                {
                    sporring = sporring.Where(h => h.Pris >= minPris);

                }

                if (maksPris > 0)
                {

                    sporring = sporring.Where(h => h.Pris <= maksPris);

                }
                if (minstAreal > 0)
                {
                    sporring = sporring.Where(h => h.areal >= minstAreal);

                }
                if (maksAreal > 0)
                {
                    sporring = sporring.Where(h => h.areal <= maksAreal);
                }
                if (minstRom > 0)
                {
                    sporring = sporring.Where(h => h.Pris >= minstRom);
                }
                if (maksRom > 0)
                {
                    sporring = sporring.Where(h => h.romAntall <= maksRom);
                }



                return await sporring.ToListAsync();

            }
            catch (Exception e)
            {
                _logger.LogError("[HomeRepo] home ToListeAsts  is Failed : error melding {e}", e.Message);
                return null;
            }
        }

        public async Task<Hus> hentHusMedId(int id)
        {

            try
            {
                return await _db.hus.FindAsync(id);

            }
            catch (Exception ex)
            {

                _logger.LogError("[HusRepo] hus til FindAsync her failet når henthus gjennom id ble kallet, error melding : {e}", ex.Message);
                return null;

            }

        }



        public async Task<bool> Lag(Hus hus)
        {
            try
            {
                _db.hus.Add(hus);
                await _db.SaveChangesAsync();
                return true;
            }

            catch (Exception ex) { 
            
                _logger.LogError("[HusRepo] feil med lagHus metoden, error melding : {e}", ex.Message);
                return false;
            }

        }

        public async Task<bool> Endre(Hus hus)
        {
            try
            {
                _db.hus.Update(hus);
                await _db.SaveChangesAsync();
                return true;
            }

            catch (Exception ex)
            {

                _logger.LogError("[HusRepo] feil med endreHus metoden, error melding : {e}", ex.Message);
                return false;
            }
        }

        public async Task<bool> Slett(int id)
        {
            try {
                var hus = await _db.hus.FindAsync(id);
                if (hus == null)
                {
                    _logger.LogError("[HusRepo] hus finnes ikke for denne iden" + id);
                    return false;
                }

                _db.hus.Remove(hus);
                await _db.SaveChangesAsync();
                return true;



            }
            catch (Exception ex)
            {

                _logger.LogError("[HusRepo] hus sletting failet for iden angitt, error melding {e}", id, ex.Message);
                return false;

            }

        }

        public Task<Hus> hentAlleMedFilter(string by, int minstAreal, int maksAreal, int minPris, int maksPris, int minstRom, int maksRom)
        {
            throw new NotImplementedException();
        }
    }
}
