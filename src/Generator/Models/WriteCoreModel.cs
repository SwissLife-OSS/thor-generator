using System.Collections.Generic;

namespace ChilliCream.Tracing.Generator.Models
{
    internal class WriteCoreModel
    {
        public int ParametersCount { get; set; }
        public List<EventParameterModel> Parameters { get; set; } = new List<EventParameterModel>();
    }
}
