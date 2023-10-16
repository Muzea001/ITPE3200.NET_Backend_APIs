using Microsoft.EntityFrameworkCore;
using Oblig1.Models;

namespace Oblig1.DAL
{
    public class KundeRepo : KundeInterface
    {
        private readonly ItemDbContext _db;

        private readonly ILogger<KundeRepo> _Kundelogger;

        public KundeRepo(ItemDbContext db, ILogger<KundeRepo> logger)
        {
            _db = db;
            _Kundelogger = logger;
        }


        public async Task<IEnumerable<Kunde>?> HentAlle()
        {
            try
            {
                return await _db.kunde.ToListAsync();
            }
            catch (Exception ex)
            {
                _Kundelogger.LogError("[KundeRepo] hentAlle kunder metoden failet ved innkalling, error melding : {e}", ex.Message);
                return null;
            }

        }


        public async Task<Kunde> hentKundeMedId(int id)
        {

            try
            {
                return await _db.kunde.FindAsync(id);

            }
            catch (Exception ex)
            {

                _Kundelogger.LogError("[KundeRepo]hent kunde med id" + id + "metoden failet ved innkalling, error melding : {e}", ex.Message);
                return null;

            }

        }



        public async Task<int> lagKunde(Kunde kunde)
        {
            try
            {
                _db.kunde.Add(kunde);
                await _db.SaveChangesAsync();
                _Kundelogger.LogError("Dette er iden til kunden");

                return kunde.kundeID;
            }

            catch (Exception ex)
            {

                _Kundelogger.LogError("[KundeRepo] feil med lagKunde metoden, error melding : {e}", ex.Message);
                return -1;
            }

        }

        public async Task<bool> endreKunde(Kunde kunde)
        {
            try
            {
                _db.kunde.Update(kunde);
                await _db.SaveChangesAsync();
                return true;
            }

            catch (Exception ex)
            {

                _Kundelogger.LogError("[KundeRepo] feil med endreKunde metoden, error melding : {e}", ex.Message);
                return false;
            }
        }

        public async Task<bool> SlettKunde(int id)
        {
            try
            {
                var kunde = await _db.kunde.FindAsync(id);
                if (kunde == null)
                {
                    _Kundelogger.LogError("[KundeRepo] kunde finnes ikke for denne iden" + id);
                    return false;
                }

                _db.kunde.Remove(kunde);
                await _db.SaveChangesAsync();
                return true;



            }
            catch (Exception ex)
            {

                _Kundelogger.LogError("[KundeRepo] kunde sletting failet for iden angitt, error melding {e}", id, ex.Message);
                return false;

            }

        }
    }
}
