using Thor.Generator.Templates;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Thor.Generator
{
    public class EventSourceModelPostProcessorTests
    {
        [Fact]
        public void CheckParameterIndex()
        {
            // arrange
            Template template = new Template(Guid.NewGuid().ToString("N"),
                Guid.NewGuid().ToString("N"), Enumerable.Empty<WriteMethod>(), 5);

            EventSourceModel eventSourceModel = new EventSourceModel();
            EventModel eventModel = new EventModel();
            eventModel.Parameters.Add(new EventParameterModel
            {
                Type = "string"
            });
            eventModel.Parameters.Add(new EventParameterModel
            {
                Type = "int"
            });
            eventSourceModel.Events.Add(eventModel);

            // act
            EventSourceModelPostProcessor postProcessor
                = new EventSourceModelPostProcessor(template);
            postProcessor.Process(eventSourceModel);

            // assert
            eventSourceModel.WriteMethods.Should().HaveCount(1);

            WriteCoreModel writeCoreModel = eventSourceModel.WriteMethods.First();
            writeCoreModel.Parameters.Should().HaveCount(2);
            writeCoreModel.Parameters.First().Position.Should().Be(5);
            writeCoreModel.Parameters.Last().Position.Should().Be(6);
        }

        [Fact]
        public void DoNotAddTemplateWriteMethods()
        {
            // arrange
            Template template = new Template(Guid.NewGuid().ToString("N"),
                Guid.NewGuid().ToString("N"), new[] { new WriteMethod(new[] { "string" }) }, 5);

            EventSourceModel eventSourceModel = new EventSourceModel();
            EventModel eventModel = new EventModel();
            eventModel.Parameters.Add(new EventParameterModel
            {
                Type = "string"
            });
            eventModel.Parameters.Add(new EventParameterModel
            {
                Type = "int"
            });
            eventSourceModel.Events.Add(eventModel);

            eventModel = new EventModel();
            eventModel.Parameters.Add(new EventParameterModel
            {
                Type = "string"
            });
            eventSourceModel.Events.Add(eventModel);

            // act
            EventSourceModelPostProcessor postProcessor
                = new EventSourceModelPostProcessor(template);
            postProcessor.Process(eventSourceModel);

            // assert
            eventSourceModel.WriteMethods.Should().HaveCount(1);

            WriteCoreModel writeCoreModel = eventSourceModel.WriteMethods.First();
            writeCoreModel.Parameters.Should().HaveCount(2);
            writeCoreModel.Parameters.First().Position.Should().Be(5);
            writeCoreModel.Parameters.Last().Position.Should().Be(6);
        }
    }
}
