using System;
using System.Collections.Generic;
using System.Text;
using ChilliCream.Logging.Generator.Models;
using Newtonsoft.Json;

namespace ChilliCream.Logging.Generator
{
    internal class EventSourceModel
    {
        public string Name { get; set; }
        public string Namespace { get; set; }
        public AttributeModel Attribute { get; set; }
        public List<EventModel> Events { get; set; } = new List<EventModel>();
        public List<WriteCoreModel> WriteMethods { get; set; } = new List<WriteCoreModel>();
    }
}
