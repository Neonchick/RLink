using Newtonsoft.Json;

namespace ResponseLIb
{
    /// <summary>
    /// Класс слова.
    /// </summary>
    public class Word
    {
        /// <summary>
        /// Границы.
        /// </summary>
        [JsonProperty("boundingBox")]
        public string BoundingBox { get; set; }
        /// <summary>
        /// Текст.
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; set; }
    }
}