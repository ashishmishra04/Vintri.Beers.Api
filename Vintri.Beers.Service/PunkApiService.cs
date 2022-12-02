using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Vintri.Beers.Model;

namespace Vintri.Beers.Service
{
    /// <summary>
    /// This Service Calls the PunkApi for Data by Id or Name
    /// </summary>
    public class PunkApiService : IPunkApiService
    {
        private readonly string _punkApiUrl;
        private readonly HttpClient _httpClient;

        public PunkApiService(string punkApiUrl)
        {
            _punkApiUrl = punkApiUrl;
            _httpClient = new HttpClient();
        }

        public async Task<Beer> Get(int id)
        {
            if (id <= 0)
                throw new ArgumentNullException(nameof(id));

            var beerRequest = $"{_punkApiUrl}/{id}";
            return await GetBeerFromPunkApi(beerRequest);
        }

        public async Task<Beer> Get(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            var beerRequest = $"{_punkApiUrl}?beer_name={name}";
            return await GetBeerFromPunkApi(beerRequest);
        }

        private async Task<Beer> GetBeerFromPunkApi(string requestUrl)
        {
            var response = await _httpClient.GetAsync(requestUrl);

            if (response.IsSuccessStatusCode)
            {
                var beerData = await response.Content.ReadAsStringAsync();
                var beers = JsonConvert.DeserializeObject<List<Beer>>(beerData);
                if (beers != null && beers.Any())
                    return beers.FirstOrDefault();
                return null;
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            var errorMessage = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error While Running URL {requestUrl}: {errorMessage}");
        }
    }
}
