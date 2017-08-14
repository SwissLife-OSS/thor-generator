using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ChilliCream.Logging.Generator
{
    internal class EventModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AttributeSyntax { get; set; }
        public List<EventParameterModel> Parameters { get; set; } = new List<EventParameterModel>();
    }
}