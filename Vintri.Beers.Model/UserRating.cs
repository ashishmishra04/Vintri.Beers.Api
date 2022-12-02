using Newtonsoft.Json;

namespace Vintri.Beers.Model
{
    /// <summary>
    /// User Rating Class Definition
    /// </summary>
    public class UserRating
    {
        /// <summary>
        /// User Name which is an valid email address
        /// </summary>
        [JsonProperty("username")]
        public string UserName { get; set; }

        /// <summary>
        /// Rating for the bear which is numeric between 1-5
        /// </summary>
        [JsonProperty("rating")]
        public int Rating { get; set; }

        /// <summary>
        /// Comments on the bear by user
        /// </summary>
        [JsonProperty("comments")]
        public string Comments { get; set; }
    }
}