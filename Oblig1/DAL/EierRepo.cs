using Microsoft.EntityFrameworkCore;
using Oblig1.Models;
using Oblig1.Services;

namespace Oblig1.DAL
{
    public class EierRepo : eierInterface
    {

        private readonly ItemDbContext _db;

        private readonly ILogger<OrdreInterface> _OrdreLogger;

        public EierRepo(ItemDbContext db, ILogger<OrdreInterface> logger)
        {
            _db = db;
            _OrdreLogger = logger;
        }


        public async Task<IEnumerable<Eier>?> HentAlle()
        {
            try
            {
                return await _db.Eier.ToListAsync();
            }
            catch (Exception ex)
            {
                _OrdreLogger.LogError("[EierRepo] hentAlle eiere metoden failet ved innkalling, error melding : {e}", ex.Message);
                return null;
            }

        }

        public async Task<Eier> hentEierMedId(long id)
        {

            try
            {
                return await _db.Eier.FindAsync(id);

            }
            catch (Exception ex)
            {

                _OrdreLogger.LogError("[EierRepo]hent eiere med id" + id + "metoden failet ved innkalling, error melding : {e}", ex.Message);
                return null;

            }

        }

        public async Task<bool> endreEier(Eier eier)
        {
            try
            {
                var existingEier = await _db.Ordre.FindAsync(eier.kontoNummer);
                if (existingEier == null)
                {
                    _OrdreLogger.LogError("[OrdreRepo] Order not found with id: {id}", eier.kontoNummer);
                    return false;
                }

                _db.Entry(existingEier).CurrentValues.SetValues(eier);
                _db.Entry(existingEier).State = EntityState.Modified;

                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _OrdreLogger.LogError("[EierRepo] Error updating eier, message: {message}", ex.Message);
                return false;
            }
        }

        public async Task<bool> SlettEier(long id)
        {
            try
            {
                var eier = await _db.Eier.FindAsync(id);
                if (eier == null)
                {
                    _OrdreLogger.LogError("[EierRepo] eier finnes ikke for denne iden" + id);
                    return false;
                }

                _db.Eier.Remove(eier);
                await _db.SaveChangesAsync();
                return true;



            }
            catch (Exception ex)
            {

                _OrdreLogger.LogError("[EierRepo] eier sletting failet for iden angitt, error melding {e}", id, ex.Message);
                return false;

            }

        }

        public async Task<Eier> hentEierMedPersonId(string personId)
        {

            var eier = await _db.Eier
                .Include(k => k.Person)
                .FirstOrDefaultAsync(k => k.Person.Id == personId);

            return eier;
        }

    }
}
