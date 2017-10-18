using System.Collections.Generic;

namespace ChilliCream.Tracing.Generator.Templates
{
    internal class WriteCoreModel
    {
        public int TotalParameters { get; set; }
        public List<WriteMethodParameterModel> Parameters { get; set; } = new List<WriteMethodParameterModel>();
    }
}
