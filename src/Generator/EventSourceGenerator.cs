using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChilliCream.Logging.Generator
{
    public class EventSourceGenerator
    {
        private static readonly WriteMethod _defaultWriteMethod = new WriteMethod(new[] { "string" });
        private readonly EventSourceDefinition _eventSourceDefinition;

        public EventSourceGenerator(EventSourceDefinition eventSourceDefinition)
        {
            if (eventSourceDefinition == null)
            {
                throw new ArgumentNullException(nameof(eventSourceDefinition));
            }

            _eventSourceDefinition = eventSourceDefinition;
        }

        public void CreateGeneratorModel ()
        {
            EventSourceModel eventSourceModel = new EventSourceModel
            {
                Name = _eventSourceDefinition.ClassName,
                Namespace = _eventSourceDefinition.Namespace
            };
            




        }



        private IEnumerable<WriteMethod> GetWriteMethods()
        {
            HashSet<WriteMethod> hashSet = new HashSet<WriteMethod>();
            foreach (EventDefinition eventDefinition in _eventSourceDefinition.Events)
            {
                hashSet.Add(new WriteMethod(eventDefinition.Arguments.Select(t => t.Type)));

            }
            return hashSet;
        }
    }
}
