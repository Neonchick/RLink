using Newtonsoft.Json;

namespace ResponseLIb
{
    public class Line
    {
        [JsonProperty("boundingBox")]
        public string BoundingBox { get; set; }
        [JsonProperty("words")]
        public Word[] Words { get; set; }
    }
}