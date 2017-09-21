using System.Collections.Generic;

namespace ChilliCream.Tracing.Generator.Models
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
