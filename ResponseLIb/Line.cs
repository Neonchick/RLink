using Newtonsoft.Json;

namespace ResponseLIb
{
    /// <summary>
    /// Класс линий.
    /// </summary>
    public class Line
    {
        /// <summary>
        /// Границы.
        /// </summary>
        [JsonProperty("boundingBox")]
        public string BoundingBox { get; set; }
        /// <summary>
        /// Список слов.
        /// </summary>
        [JsonProperty("words")]
        public Word[] Words { get; set; }
    }
}