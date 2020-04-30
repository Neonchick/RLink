using Newtonsoft.Json;

namespace ResponseLIb
{
    public class Region
    {
        [JsonProperty("boundingBox")]
        public string BoundingBox { get; set; }
        [JsonProperty("lines")]
        public Line[] Lines { get; set; }
    }
}