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


        public async Task<IEnumerable<Hus>?> hentAlle()
        {
            try
            {
                return await _db.hus.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("[HusRepo] hus ToListAsync failet når hentalle ble kallet, error melding : {e}", ex.Message);
                return null;
            }

        }


        public async Task<IEnumerable<Hus>?> HentAlleTilgjengelig()
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
    }
}
