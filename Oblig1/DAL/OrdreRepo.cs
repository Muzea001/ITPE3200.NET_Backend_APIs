using Microsoft.EntityFrameworkCore;
using Oblig1.Models;

namespace Oblig1.DAL
{
    public class OrdreRepo : OrdreInterface

    {

        private readonly ItemDbContext _db;

        private readonly ILogger<OrdreInterface> _OrdreLogger;

        public OrdreRepo(ItemDbContext db, ILogger<OrdreInterface> logger)
        {
            _db = db;
            _OrdreLogger = logger;
        }


        public async Task<IEnumerable<Ordre>?> HentAlle()
        {
            try
            {
                return await _db.ordre.ToListAsync();
            }
            catch (Exception ex)
            {
                _OrdreLogger.LogError("[OrdreRepo] hentAlle ordre metoden failet ved innkalling, error melding : {e}", ex.Message);
                return null;
            }

        }


        public async Task<Ordre> hentOrdreMedId(int id)
        {

            try
            {
                return await _db.ordre.FindAsync(id);

            }
            catch (Exception ex)
            {

                _OrdreLogger.LogError("[OrdreRepo]hent ordre med id" + id + "metoden failet ved innkalling, error melding : {e}", ex.Message);
                return null;

            }

        }



        public async Task<bool> lagOrdre(Ordre ordre, int husID, Kunde kunde)
        {
            try
            {
                var hus = _db.hus.FindAsync(husID);
                if (hus != null)
                {
                    hus. = false;
                    ordre.kundeID = kunde.kundeID;
                    ordre.husId = husID;
                    _db.ordre.Add(ordre);
                    await _db.SaveChangesAsync();
                    return true;
                }

                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

                _OrdreLogger.LogError("[OrdreRepo] feil med lagOrdre metoden, error melding : {e}", ex.Message);
                return false;
            }

        }

        public async Task<bool> endreOrdre(Ordre ordre)
        {
            try
            {
                _db.ordre.Update(ordre);
                await _db.SaveChangesAsync();
                return true;
            }

            catch (Exception ex)
            {

                _OrdreLogger.LogError("[OrdreRepo] feil med endreKunde metoden, error melding : {e}", ex.Message);
                return false;
            }
        }

        public async Task<bool> SlettOrdre(int id)
        {
            try
            {
                var ordre = await _db.ordre.FindAsync(id);
                if (ordre == null)
                {
                    _OrdreLogger.LogError("[OrdreRepo] ordre finnes ikke for denne iden" + id);
                    return false;
                }

                _db.ordre.Remove(ordre);
                await _db.SaveChangesAsync();
                return true;



            }
            catch (Exception ex)
            {

                _OrdreLogger.LogError("[OrdreRepo] ordre sletting failet for iden angitt, error melding {e}", id, ex.Message);
                return false;

            }

        }
    }
}
