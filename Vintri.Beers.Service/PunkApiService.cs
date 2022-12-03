using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using log4net;
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
        private readonly ILog _log = LogManager.GetLogger(nameof(PunkApiService));

        /// <summary>
        /// Constructor for PunkApiService
        /// </summary>
        /// <param name="punkApiUrl">Punk Api Url for Beer</param>
        public PunkApiService(string punkApiUrl)
        {
            _punkApiUrl = punkApiUrl;
            _httpClient = new HttpClient();
        }

        private async Task<Beer> GetBeerFromPunkApi(string requestUrl)
        {
            _log.Debug($"Calling API Url: {requestUrl}");

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
            var message = $"Error While Running URL {requestUrl}: {errorMessage}";
            _log.Error(message);
            throw new Exception(message);
        }

        /// <summary>
        /// Get the data from Punk Api by Id
        /// </summary>
        /// <param name="id">id of the beer</param>
        /// <returns>Beer data Object</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<Beer> Get(int id)
        {
            if (id <= 0)
                throw new ArgumentNullException(nameof(id));

            var beerRequest = $"{_punkApiUrl}/{id}";
            return await GetBeerFromPunkApi(beerRequest);
        }

        /// <summary>
        /// Get the Beer by name of the beer
        /// </summary>
        /// <param name="name">name of the beer</param>
        /// <returns>Beer data Object</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<Beer> Get(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            var beerRequest = $"{_punkApiUrl}?beer_name={name}";
            return await GetBeerFromPunkApi(beerRequest);
        }
    }
}
