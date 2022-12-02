using System.Collections.Generic;
using System.Threading.Tasks;
using Vintri.Beers.Model;

namespace Vintri.Beers.Service
{
    public interface IBeerService
    {
        Task<Beer> Get(int id);
        Task<Beer> Get(string name);
        Task<Beer> AddUserRating(int id, UserRating userRating);
    }
}