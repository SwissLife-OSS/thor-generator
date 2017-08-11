using System;
using System.Collections.Generic;
using System.Text;
using ChilliCream.Logging.Generator.Models;
using Newtonsoft.Json;

namespace ChilliCream.Logging.Generator
{
    internal class EventSourceModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("namespace")]
        public string Namespace { get; set; }

        [JsonProperty("attributeArgumentSyntax")]
        public string AttributeArgumentSyntax { get; set; }

        [JsonProperty("events")]
        public List<EventModel> Events { get; set; } = new List<EventModel>();

        [JsonProperty("writeMethods")]
        public List<WriteCoreModel> WriteMethods { get; set; } = new List<WriteCoreModel>();
    }
}
