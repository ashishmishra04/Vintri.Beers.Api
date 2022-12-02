using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using System.Web.Http;
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

        /// <summary>
        /// Constructor For Beers Controller
        /// </summary>
        public BeersController()
        {
            var databaseFullPath  = Path.Combine(_rootFolder, ConfigurationManager.AppSettings["databaseFilePath"]);
            _service = new BeerService(ConfigurationManager.AppSettings["punkApiUrl"], databaseFullPath);
        }

        /// <summary>
        /// Get beer by Id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetById")]
        public async Task<IHttpActionResult> GetById(int id)
        {
            var beer = await _service.Get(id);

            if (beer == null)
                return NotFound();

            return Ok(beer);
        }

        /// <summary>
        /// Get beer by name
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetByName")]
        public async Task<IHttpActionResult> GetByName(string name)
        {
            var beer = await _service.Get(name);

            if (beer == null)
                return NotFound();

            return Ok(beer);
        }

        /// <summary>
        /// Add UserRating to beer by Id
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("AddUserRating")]
        public async Task<IHttpActionResult> Post(int id, UserRating userRating)
        {
            if (!ModelState.IsValid) 
                return BadRequest();
            
            var beer = await _service.AddUserRating(id, userRating);
            return Ok(beer);

        }

        /// <summary>
        /// Get All beers with rating information
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllBeerWithRating")]
        public async Task<IHttpActionResult> GetAllBeerWithRating()
        {
            var beers = await _service.GetAllBeerWithRating();
            return Ok(beers);
        }
    }
}