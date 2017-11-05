using System.Linq;
using System.Collections.Generic;

namespace ChilliCream.Tracing.Generator.Templates
{
    internal class EventSourceModel
    {
        public string FileName { get; set; }
        public string Name { get; set; }
        public string Namespace { get; set; }

        public string InterfaceName { get; set; }

        public AttributeModel Attribute { get; } = new AttributeModel(Constants.EventSourceAttributeName);
        public List<EventModel> Events { get; } = new List<EventModel>();
        public List<WriteCoreModel> WriteMethods { get; } = new List<WriteCoreModel>();

        public bool HasEvents => Events.Any();
        public bool HasWriteMethods => WriteMethods.Any();
    }
}