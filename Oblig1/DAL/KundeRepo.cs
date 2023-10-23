using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
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
                return await _db.Kunde.ToListAsync();
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
                return await _db.Kunde.FindAsync(id);

            }
            catch (Exception ex)
            {

                _Kundelogger.LogError("[KundeRepo]hent kunde med id" + id + "metoden failet ved innkalling, error melding : {e}", ex.Message);
                return null;

            }

        }


        public async Task<bool> endreKunde(Kunde kunde)
        {
            try
            {
                var existingKunde = await _db.Kunde
                    .Include(k => k.Person)
                    .FirstOrDefaultAsync(k => k.kundeID == kunde.kundeID);

                if (existingKunde == null)
                {
                    _Kundelogger.LogError("[OrdreRepo] Kunde not found with id: {id}", kunde.kundeID);
                    return false;
                }

                if (existingKunde.Person != null && kunde.Person != null)
                {
                    // Explicitly updating the Person entity
                    _db.Entry(existingKunde.Person).CurrentValues.SetValues(kunde.Person);
                    _db.Entry(existingKunde.Person).State = EntityState.Modified;
                }

                _db.Entry(existingKunde).CurrentValues.SetValues(kunde);
                _db.Entry(existingKunde).State = EntityState.Modified;

                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _Kundelogger.LogError("[OrdreRepo] Error updating kunde, message: {message}", ex.Message);
                return false;
            }
        }

        public async Task<bool> SlettKunde(int id)
        {
            try
            {
                var kunde = await _db.Kunde.FindAsync(id);
                if (kunde == null)
                {
                    _Kundelogger.LogError("[KundeRepo] kunde finnes ikke for denne iden" + id);
                    return false;
                }

                _db.Kunde.Remove(kunde);
                await _db.SaveChangesAsync();
                return true;



            }
            catch (Exception ex)
            {

                _Kundelogger.LogError("[KundeRepo] kunde sletting failet for iden angitt, error melding {e}", id, ex.Message);
                return false;

            }

        }

        public Task<int> lagKunde(Kunde kunde)
        {
            throw new NotImplementedException();
        }

        public Task<Kunde> finnKundeId(string id)
        {
            throw new NotImplementedException();
        }
    }
}
