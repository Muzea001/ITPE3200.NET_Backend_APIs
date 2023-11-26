using Oblig1.Models;

namespace Oblig1.Services
{
    public interface PersonInterface
    {


        Task<bool> endrePerson(Person person);
        Task<IEnumerable<Person>?> HentAlle();
        Task<Person> hentPersonMedId(string id);
        Task<string> Lag(Person person);

        Task<Person> hentPersonMedEmail(string email);
        Task<bool> SlettPerson(string id);
    }
}
