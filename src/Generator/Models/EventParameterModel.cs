using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ChilliCream.Logging.Generator
{
    internal class EventParameterModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("size")]
        public string Size { get; set; }

        [JsonProperty("isString")]
        public bool IsString { get; set; }

        [JsonProperty("operator")]
        public string Operator { get; set; }

        [JsonProperty("position")]
        public int? Position { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("isFollowing")]
        public bool IsFollowing { get; set; }
    }
}
