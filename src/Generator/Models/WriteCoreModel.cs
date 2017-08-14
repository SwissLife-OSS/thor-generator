using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ChilliCream.Logging.Generator.Models
{
    internal class WriteCoreModel
    {
        public int ParametersCount { get; set; }
        public List<EventParameterModel> Parameters { get; set; } = new List<EventParameterModel>();
    }
}
