using Newtonsoft.Json;

namespace ResponseLIb
{
    /// <summary>
    /// Класс распознования.
    /// </summary>
    public class Response
    {
        /// <summary>
        /// Язык.
        /// </summary>
        [JsonProperty("language")]
        public string Language { get; set; }
        /// <summary>
        /// Угол.
        /// </summary>
        [JsonProperty("textAngle")]
        public double TextAngle { get; set; }
        /// <summary>
        /// Орентация.
        /// </summary>
        [JsonProperty("orientation")]
        public string Orientation { get; set; }
        /// <summary>
        /// Массив регионов.
        /// </summary>
        [JsonProperty("regions")]
        public Region[] Regions { get; set; }
    }
}