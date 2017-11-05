using System.Collections.Generic;
using System.Linq;

namespace Thor.Generator.Templates
{
    internal class EventModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public AttributeModel Attribute { get; } = new AttributeModel("Event");

        public List<EventParameterModel> Parameters { get; } = new List<EventParameterModel>();

        public bool HasParameters => Parameters != null && Parameters.Any();
    }
}
