using System.Threading.Tasks;
using Vintri.Beers.Model;

namespace Vintri.Beers.Service
{
    public interface IPunkApiService
    {
        Task<Beer> Get(int id);
        Task<Beer> Get(string name);
    }
}