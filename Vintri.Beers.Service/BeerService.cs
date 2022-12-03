using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Vintri.Beers.Model;
using Vintri.Beers.Model.Validation;

namespace Vintri.Beers.Service
{
    public class BeerService : IBeerService
    {
        private readonly string _storageLocationPath;
        private readonly IPunkApiService _punkApiService;
        readonly ObjectCache _cache = MemoryCache.Default;
        private readonly string _cacheName = "beerDatabase";
        private readonly List<BeerRating> _beersWithRating = new List<BeerRating>();

        /// <summary>
        /// Constructor For Beer Service
        /// </summary>
        /// <param name="punkApiUrl">Punk API URL</param>
        /// <param name="storageLocationPath">Storage location for the Json file to save User Ratings</param>
        public BeerService(string punkApiUrl, string storageLocationPath)
        {
            _storageLocationPath = storageLocationPath;
            _punkApiService = new PunkApiService(punkApiUrl);

            // The cache policy is added to the file with expiry of 30 sec
            var policy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(30.0)
            };
            // ChangeMonitor will clear the cache upon file change
            policy.ChangeMonitors.Add(new HostFileChangeMonitor(new List<string> {_storageLocationPath}));

            // Set the Cache Value to the local Storage Path
            var fileContents = File.ReadAllText(_storageLocationPath);
            _cache.Set(_cacheName, fileContents, policy);

            // Load the Local stored Beers 
            if (_cache[_cacheName] is string beerDatabaseContent)
                _beersWithRating = JsonConvert.DeserializeObject<List<BeerRating>>(beerDatabaseContent);
        }

        /// <summary>
        /// Validate the User Rating Data Object
        /// </summary>
        /// <param name="userRating"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        private ValidationResult ValidateUserRatingDataObject(UserRating userRating)
        {
            var validationResult = new ValidationResult();
            if (userRating == null)
            {
                validationResult.AddError(new ValidationError("UserRating not Provided", nameof(userRating)));
                return validationResult;
            }

            if (!IsValidRating(userRating.Rating))
                validationResult.AddError(new ValidationError($"Invalid Rating provided: {userRating.Rating}, valid number is 1 to 5", nameof(userRating.Rating)));
               

            if (!string.IsNullOrEmpty(userRating.UserName) && !IsValidEmail(userRating.UserName))
                validationResult.AddError(new ValidationError($"Invalid username provided: {userRating.UserName}, valid email required", nameof(userRating.UserName)));

            return validationResult;
        }

        //Make sure its a valid email address
        private bool IsValidEmail(string email)
        {
            var validateEmailRegex = new Regex("^\\S+@\\S+\\.\\S+$");

            // More Accurate way would be to use 
            // MailAddress m = new MailAddress(email); and handle try catch
            return validateEmailRegex.IsMatch(email);
        }

        //Make sure the rating is between 1 to 5
        private bool IsValidRating(int rating)
        {
            return rating > 0 && rating <= 5;
        }

        /// <summary>
        /// Get All beers with rating information
        /// </summary>
        /// <returns>List of beer</returns>
        public async Task<List<Beer>> GetAllBeerWithRating()
        {
            var beersBag = new ConcurrentBag<Beer>();
            var tasks = _beersWithRating.Select(async beersWithRatingItem =>
            {
                var beer = await Get(beersWithRatingItem.Id);
                if (beer == null) return;

                beer.UserRatings = _beersWithRating.FirstOrDefault(beerItem => beerItem.Id == beer.Id)?.UserRatings;
                beersBag.Add(beer);
            });
            await Task.WhenAll(tasks);
            return beersBag.ToList();
        }

        /// <summary>
        /// Get the Beer with the id provided
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Beer> Get(int id)
        {
            var beer = await _punkApiService.Get(id);
            if (beer != null)
                beer.UserRatings = _beersWithRating.FirstOrDefault(beerItem => beerItem.Id == beer.Id)?.UserRatings;

            return beer;
        }

        /// <summary>
        /// Get the Beer with the name provided
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<Beer> Get(string name)
        {
            var beer  = await _punkApiService.Get(name);
            if (beer != null)
                beer.UserRatings = _beersWithRating.FirstOrDefault(beerItem => beerItem.Id == beer.Id)?.UserRatings;

            return beer;
        }

        /// <summary>
        /// Update The Beer rating with id of the beer
        /// </summary>
        /// <param name="id">Beer Id</param>
        /// <param name="userRating">UserRating Data Object</param>
        /// <returns></returns>
        public async Task<OperationResult<Beer>> AddUserRating(int id, UserRating userRating)
        {
            var beer = await _punkApiService.Get(id);
            if (beer == null) return new OperationResult<Beer>(data: null, 
                new ValidationResult(
                    new List<ValidationError>
                        { new ValidationError($"Invalid beer Id provided {id}", nameof(id))}));

            //Validation for User Rating data Object
            var validationResult = ValidateUserRatingDataObject(userRating);

            if (!validationResult.IsValid)
                return new OperationResult<Beer>(data: null, validationResult: validationResult);

            var existingBeerRating = _beersWithRating.FirstOrDefault(beerItem => beerItem.Id == beer.Id);
            if (existingBeerRating == null)
            {
                _beersWithRating.Add(new BeerRating
                {
                    Id = id,
                    UserRatings = new List<UserRating> {userRating}
                });
            }
            else
            {
                // FIND EXISTING USER RATING FOR SAME BEER BY USERNAME TO UPDATE OR CREATE NEW RATING
                var existingUserRating = existingBeerRating.UserRatings.FirstOrDefault(rating =>
                    rating.UserName.Equals(userRating.UserName, StringComparison.CurrentCultureIgnoreCase));

                // UPDATE USER RATING AND COMMENTS IF A RECORD ALREADY EXISTS
                if (existingUserRating != null)
                {
                    existingUserRating.Comments = userRating.Comments;
                    existingUserRating.Rating = userRating.Rating;
                }
                //CREATE A NEW USER COMMENT
                else
                {
                    existingBeerRating.UserRatings.Add(userRating);
                }
            }

            //SAVE THE FILE
            var serializeBeers = JsonConvert.SerializeObject(_beersWithRating, Formatting.Indented);
            File.WriteAllText(_storageLocationPath, serializeBeers);

            return new OperationResult<Beer>(data: await Get(id), validationResult: new ValidationResult());
        }
    }
}
