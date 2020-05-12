using Newtonsoft.Json;

namespace ResponseLIb
{
    /// <summary>
    /// Класс региона.
    /// </summary>
    public class Region
    {
        // Границы.
        [JsonProperty("boundingBox")]
        public string BoundingBox { get; set; }
        // Массив линий.
        [JsonProperty("lines")]
        public Line[] Lines { get; set; }
    }
}