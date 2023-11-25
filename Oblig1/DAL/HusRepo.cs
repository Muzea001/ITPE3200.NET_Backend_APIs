using Microsoft.AspNetCore.Authorization;
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
                return await _db.Hus.Include(h => h.bildeListe).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("[HusRepo] hus ToListAsync failet når hentalle ble kallet, error melding : {e}", ex.Message);
                return null;
            }

        }

        public async Task<IEnumerable<Hus>> hentMine(string email)
        {
            try
            {
                var person = await _db.Person.FirstOrDefaultAsync(k => k.Email == email);
                var eier = await _db.Eier
                    .Include(k => k.Person)
                    .FirstOrDefaultAsync(k => k.Person.Id == person.Id);
                var Liste = await _db.Hus.Where(h => h.eier.kontoNummer == eier.kontoNummer).ToListAsync();
                return Liste;
            }
            catch (Exception ex)
            {
                _logger.LogError("[HusRepo] metoden hent mine hus failed ved inkalling, error message: {e}", ex.Message);
                return null;
            }
        }


        public async Task<IEnumerable<Hus>> HentAlleTilgjengelig()
        {
            try
            {
                return await _db.Hus.FromSqlRaw("SELECT* FROM hus WHERE erTilgjengelig=1").ToListAsync();
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
                var sporring = _db.Hus.AsQueryable();

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
                return await _db.Hus.FindAsync(id);

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
                _db.Hus.Add(hus);
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
                var existingHus = await _db.Hus.FindAsync(hus.husId);
                if (existingHus == null)
                {
                    _logger.LogError("[HusRepo] House not found with id: {id}", hus.husId);
                    return false;
                }

                _db.Entry(existingHus).CurrentValues.SetValues(hus);
                _db.Entry(existingHus).State = EntityState.Modified;

                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("[HusRepo] Error updating house, message: {message}", ex.Message);
                return false;
            }
        }

        public async Task<bool> Slett(int id)
        {
            try {
                var hus = await _db.Hus.Include(h => h.ordreListe)
                        .Include(h => h.bildeListe)
                        .FirstOrDefaultAsync(h => h.husId == id);
                if (hus == null)
                {
                    _logger.LogError("[HusRepo] hus finnes ikke for denne iden" + id);
                    return false;
                }
                _db.Ordre.RemoveRange(hus.ordreListe);
                _db.Bilder.RemoveRange(hus.bildeListe);
                _db.Hus.Remove(hus);
                await _db.SaveChangesAsync();
                return true;



            }
            catch (Exception ex)
            {

                _logger.LogError("[HusRepo] hus sletting failet for iden angitt, error melding {e}", id, ex.Message);
                return false;

            }

        }

        public async Task<bool> SlettEier(int id)
        {
            try
            {
                var eier = await _db.Eier.FindAsync(id);
                if (eier == null)
                {
                    _logger.LogError("[HusRepo] hus finnes ikke for denne iden" + id);
                    return false;
                }

                _db.Eier.Remove(eier);
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
