using System.Collections.Generic;
using System.Threading.Tasks;
using Vintri.Beers.Model;
using Vintri.Beers.Model.Validation;

namespace Vintri.Beers.Service
{
    public interface IBeerService
    {
        Task<List<Beer>> GetAllBeerWithRating();
        Task<Beer> Get(int id);
        Task<Beer> Get(string name);
        Task<OperationResult<Beer>> AddUserRating(int id, UserRating userRating);
    }
}