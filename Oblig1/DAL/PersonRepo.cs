using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Oblig1.Models;
using Oblig1.Services;

namespace Oblig1.DAL
{
   

    public class PersonRepo : PersonInterface
    {
        private readonly ItemDbContext _db;

        private readonly ILogger<PersonRepo> _PersonLogger;

        public PersonRepo(ItemDbContext db, ILogger<PersonRepo> logger)
        {
            _db = db;
            _PersonLogger = logger;
        }


        public async Task<IEnumerable<Person>?> HentAlle()
        {
            try
            {
                return await _db.Person.ToListAsync();
            }
            catch (Exception ex)
            {
                _PersonLogger.LogError("[KundeRepo] hentAlle kunder metoden failet ved innkalling, error melding : {e}", ex.Message);
                return null;
            }

        }


        public async Task<Person> hentPersonMedId(string id)
        {

            try
            {
                var person = await _db.Person
            .FirstOrDefaultAsync(k => k.Email == id); 
                return person;

            }
            catch (Exception ex)
            {

                _PersonLogger.LogError("[PersonRepo]hent person med id" + id + "metoden failet ved innkalling, error melding : {e}", ex.Message);
                return null;

            }

        }

        public async Task<Person> hentPersonMedEmail(string email){

            try
            {
                var person = await _db.Person.FirstOrDefaultAsync(k => k.Email == email);
                return person;
            }
            catch
            {
                _PersonLogger.LogError("Denne personene finnes ikke");
                return null;
            }

        }


        public async Task<bool> endrePerson(Person person)
        {
            try
            {
                var existingPerson = await _db.Person.FindAsync(person.Id);
                if (existingPerson == null)
                {
                    _PersonLogger.LogError("[OrdreRepo] Order not found with id: {id}", person.Id);
                    return false;
                }

                _db.Entry(existingPerson).CurrentValues.SetValues(person);
                _db.Entry(existingPerson).State = EntityState.Modified;

                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _PersonLogger.LogError("[PersonRepo] Error updating person, message: {message}", ex.Message);
                return false;
            }
        }

        public async Task<bool> SlettPerson(string id)
        {
            try
            {
                var person = await _db.Person.FindAsync(id);
                if (person == null)
                {
                    _PersonLogger.LogError("[PersonRepo] person finnes ikke for denne iden" + id);
                    return false;
                }

                _db.Person.Remove(person);
                await _db.SaveChangesAsync();
                return true;



            }
            catch (Exception ex)
            {

                _PersonLogger.LogError("[PersonRepo] person sletting failet for iden angitt, error melding {e}", id, ex.Message);
                return false;

            }

        }
        public async Task<string> Lag(Person person)
        {
            try
            {
                _db.Person.Add(person);
                await _db.SaveChangesAsync();
                return person.Id;
            }
            catch (Exception ex)
            {
                _PersonLogger.LogError("[KundeRepo] Error in Lag method, error message: {e}", ex.Message);
                return null;
            }
        }



    }
}
