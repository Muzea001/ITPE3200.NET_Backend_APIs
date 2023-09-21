using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oblig1.Models;
using Oblig1.Services;
using System.Linq.Expressions;

namespace Oblig1.DAL
{
    public class BrukerRepo : BrukerInterface
    {
        private readonly ItemDbContext _db;

        private readonly ILogger<BrukerRepo> _Brukerlogger;

        public BrukerRepo(ItemDbContext db, ILogger<BrukerRepo> logger)
        {
            _db = db;
            _Brukerlogger = logger;
        }


        public async Task<IEnumerable<Bruker>?> HentAlle()
        {
            try
            {
                return await _db.bruker.ToListAsync();
            }
            catch (Exception ex)
            {
                _Brukerlogger.LogError("[BrukerRepo] hus ToListAsync failet når hentalle ble kallet, error melding : {e}", ex.Message);
                return null;
            }

        }



        public async Task<IEnumerable<Bruker>?> HentAlleAdmins()
        {
            try
            {
                return await _db.bruker.FromSqlRaw("SELECT* FROM bruker WHERE erAdmin = 1").ToListAsync();
            }
            catch (Exception ex)
            {
                _Brukerlogger.LogError("[BrukerRepo] hentAlleAdmins ToListAsync failet når hentalleadmins ble kallet, error melding : {e}", ex.Message);
                return null;
            }

        }





        public async Task<IEnumerable<Bruker>?> HentAlleNonAdmins()
        {
            try
            {
                return await _db.bruker.FromSqlRaw("SELECT* FROM bruker WHERE erAdmin = 0").ToListAsync();
            }
            catch (Exception ex)
            {
                _Brukerlogger.LogError("[BrukerRepo] hentAlleAdmins ToListAsync failet når hentalleadmins ble kallet, error melding : {e}", ex.Message);
                return null;
            }

        }


        public async Task<IEnumerable<Bruker>?> HentAlleEiere()
        {
            try
            {
                return await _db.bruker.FromSqlRaw("SELECT* FROM bruker WHERE erEier = 1").ToListAsync();
            }
            catch (Exception ex)
            {
                _Brukerlogger.LogError("[BrukerRepo] hentAlleEiere ToListAsync failet når hentalleEiere ble kallet, error melding : {e}", ex.Message);
                return null;
            }

        }

        public async Task<Bruker> hentBrukerMedId(int brukerid)
        {

            try
            {
                return await _db.bruker.FindAsync(brukerid);

            }
            catch (Exception ex)
            {

                _Brukerlogger.LogError("[BrukerRepo] bruker til FindAsync her failet når hentbruker gjennom id ble kallet, error melding : {e}", ex.Message);
                return null;

            }

        }


        


        public async Task<bool> lagBruker(Bruker bruker)
        {
            try
            {
                _db.bruker.Add(bruker);
                await _db.SaveChangesAsync();
                return true;
            }

            catch (Exception ex)
            {

                _Brukerlogger.LogError("[BrukerRepo] feil med lagBruker metoden, error melding : {e}", ex.Message);
                return false;
            }

        }

        public async Task<bool> endreBruker(Bruker bruker)
        {
            try
            {
                _db.bruker.Update(bruker);
                await _db.SaveChangesAsync();
                return true;
            }

            catch (Exception ex)
            {

                _Brukerlogger.LogError("[BrukerRepo] feil med endreHus metoden, error melding : {e}", ex.Message);
                return false;
            }
        }

        public async Task<bool> EndreBrukerStatus(int brukerId, bool OK ) {
        
            try
            {
                var bruker = await _db.bruker.FindAsync(brukerId);
                if (bruker != null)
                {
                    bruker.erAdmin = true;
                    await _db.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
                
            }
            catch( Exception e)
            {
                _Brukerlogger.LogError("[BrukerRepo] feil med gjør bruker til kunde metode, error melding : {e}", e.Message);
                return false;
            }
        }


     

        public async Task<bool> Slett(int brukerid)
        {
            try
            {
                var bruker = await _db.bruker.FindAsync(brukerid);
                if (bruker == null)
                {
                    _Brukerlogger.LogError("[BrukerRepo] Bruker finnes ikke for dette brukernavnet", brukerid);
                    return false;
                }

                _db.bruker.Remove(bruker);
                await _db.SaveChangesAsync();
                return true;



            }
            catch (Exception ex)
            {

                _Brukerlogger.LogError("[BrukerRepo] hus sletting failet for iden angitt, error melding {e}",brukerid, ex.Message);
                return false;

            }

        }
    }
}
