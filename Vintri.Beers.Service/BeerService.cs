using System;
using System.Threading.Tasks;
using Vintri.Beers.Model;

namespace Vintri.Beers.Service
{
    public class BeerService : IBeerService
    {
        private readonly string _storageLocationPath;
        private readonly IPunkApiService _punkApiService;
        public BeerService(string punkApiUrl, string storageLocationPath)
        {
            _storageLocationPath = storageLocationPath;
            _punkApiService = new PunkApiService(punkApiUrl);
        }

        public async Task<Beer> Get(int id)
        {
            return await _punkApiService.Get(id);
        }

        public async Task<Beer> Get(string name)
        {
            return await _punkApiService.Get(name);
        }

        public async Task<Beer> AddUserRating(int id, UserRating userRating)
        {
            throw new NotImplementedException();
        }
    }
}
