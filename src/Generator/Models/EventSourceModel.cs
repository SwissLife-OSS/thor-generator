using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ChilliCream.Logging.Generator
{
    internal class EventSourceModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("namespace")]
        public string Namespace { get; set; }

        [JsonProperty("events")]
        public List<EventModel> Events { get; set; } = new List<EventModel>();
    }
}
