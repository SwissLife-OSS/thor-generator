using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ChilliCream.Logging.Generator
{
    internal class EventParameterModel
    {
        public string Name { get; set; }
        public string Size { get; set; }
        public bool IsString { get; set; }
        public string Operator { get; set; }
        public int? Position { get; set; }
        public string Type { get; set; }
        public bool IsFollowing { get; set; }
    }
}
