using Newtonsoft.Json;

namespace ResponseLIb
{
    public class Response
    {
        [JsonProperty("language")]
        public string Language { get; set; }
        [JsonProperty("textAngle")]
        public double TextAngle { get; set; }
        [JsonProperty("orientation")]
        public string Orientation { get; set; }
        [JsonProperty("regions")]
        public Region[] Regions { get; set; }
    }
}