using System.Collections.Generic;
using System.Linq;

namespace Thor.Generator.Templates
{
    internal class EventModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DocumentationXml { get; set; }
        public AttributeModel Attribute { get; } = new AttributeModel("Event");
        public List<EventParameterModel> InputParameters { get; } = new List<EventParameterModel>();
        public List<EventParameterModel> ValueParameters { get; } = new List<EventParameterModel>();
        public List<EventParameterModel> ComplexParameters { get; } = new List<EventParameterModel>();
        public bool HasParameters => InputParameters != null && InputParameters.Any();
        public bool HasComplexTypeParameters => ComplexParameters.Any();
    }
}
