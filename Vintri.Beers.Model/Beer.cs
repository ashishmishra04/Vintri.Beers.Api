using System.Collections.Generic;
using Newtonsoft.Json;

namespace Vintri.Beers.Model
{
    /// <summary>
    /// Bear Class Definition
    /// </summary>
    public class Beer : BaseUserRatings
    {
        /// <summary>
        /// Unique Identifier for the beer
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Name of the beer
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Description for the beer
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
