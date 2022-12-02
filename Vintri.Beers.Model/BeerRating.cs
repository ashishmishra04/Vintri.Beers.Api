using Newtonsoft.Json;

namespace Vintri.Beers.Model
{
    /// <summary>
    /// Beer Rating class used for database.json
    /// </summary>
    public class BeerRating : BaseUserRatings
    {
        /// <summary>
        /// Unique Identifier for the beer
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }
    }
}
