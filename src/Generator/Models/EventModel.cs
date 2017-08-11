using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ChilliCream.Logging.Generator
{
    internal class EventModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("attributeSyntax")]
        public string AttributeSyntax { get; set; }

        [JsonProperty("parameters")]
        public List<EventParameterModel> Parameters { get; set; } = new List<EventParameterModel>();
    }
}
