using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ChilliCream.Logging.Generator.Models
{
    internal class WriteCoreModel
    {
        [JsonProperty("parameters-count")]
        public int ParametersCount => Parameters.Count;

        [JsonProperty("parameters")]
        public List<EventParameterModel> Parameters { get; set; } = new List<EventParameterModel>();
    }
}
