using Oblig1.Models;

namespace Oblig1.Services
{
    public interface eierInterface
    {
       
        Task<IEnumerable<Eier>?> HentAlleEiere();
        
    }
}