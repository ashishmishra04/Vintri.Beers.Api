using System.Collections.Generic;
using Newtonsoft.Json;

namespace Vintri.Beers.Model
{
    public class BaseUserRatings
    {
        /// <summary>
        /// List of User Ratings provided by users on a beer
        /// </summary>
        [JsonProperty("userRatings")]
        public List<UserRating> UserRatings { get; set; } = new List<UserRating>();
    }
}
