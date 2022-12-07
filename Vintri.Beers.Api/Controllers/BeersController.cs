using System;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using log4net;
using Newtonsoft.Json;
using Vintri.Beers.Model;
using Vintri.Beers.Service;

namespace Vintri.Beers.Api.Controllers
{
    /// <summary>
    /// Beers Api Controller 
    /// </summary>
    public class BeersController: ApiController
    {
        /// <summary>
        /// Local Private service instance
        /// </summary>
        private readonly IBeerService _service;

        private readonly string _rootFolder = AppDomain.CurrentDomain.BaseDirectory;

        private readonly ILog _log;

        /// <summary>
        /// Constructor For Beers Controller
        /// </summary>
        public BeersController()
        {
            var databaseFullPath  = Path.Combine(_rootFolder, ConfigurationManager.AppSettings["databaseFilePath"]);
            _service = new BeerService(ConfigurationManager.AppSettings["punkApiUrl"], databaseFullPath);
            _log = LogManager.GetLogger(typeof(BeersController));
        }

        /// <summary>
        /// Get beer by Id, the search is done against PunkApi for the id and user ratings is added if its added to the local database file
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetById")]
        public async Task<IHttpActionResult> GetById(int id)
        {

            _log.Info($"Calling GetById: {id}");

            var beer = await _service.Get(id);
            if (beer == null)
                return NotFound();

            return Ok(beer);
        }

        /// <summary>
        /// Get beer by name, the search is done against PunkApi for the name and user ratings is added if its added to the local database file
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetByName")]
        public async Task<IHttpActionResult> GetByName(string name)
        {
            _log.Info($"Calling GetByName: {name}");

            var beer = await _service.Get(name);

            if (beer == null)
                return NotFound();

            return Ok(beer);
        }

        /// <summary>
        /// Add UserRating to beer by Id, the search for the beer is done against PunkApi and if data found then the rating is added to the local database with one comments per username
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidUserFilter]
        [Route("AddUserRating")]
        public async Task<IHttpActionResult> Post(int id, UserRating userRating)
        {

            _log.Info($"Calling AddUserRating for Id: {id}, with UserRating :{JsonConvert.SerializeObject(userRating)}");

            if (!ModelState.IsValid) 
                return BadRequest();
            
            var operationResult = await _service.AddUserRating(id, userRating);
            return Ok(operationResult);

        }

        /// <summary>
        /// Get All beers with rating information which is stored locally
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllBeerWithRating")]
        public async Task<IHttpActionResult> GetAllBeerWithRating()
        {
            _log.Info("Calling GetAllBeerWithRating");

            var beers = await _service.GetAllBeerWithRating();
            return Ok(beers);
        }
    }
}