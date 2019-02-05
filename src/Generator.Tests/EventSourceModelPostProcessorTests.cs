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
                Guid.NewGuid().ToString("N"), Enumerable.Empty<WriteMethod>(), Enumerable.Empty<NamespaceModel>(), 5, false, null);

            EventSourceModel eventSourceModel = new EventSourceModel();
            EventModel eventModel = new EventModel();
            eventModel.InputParameters.Add(new EventParameterModel
            {
                Type = "string"
            });
            eventModel.InputParameters.Add(new EventParameterModel
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
        public void ReplaceNamedParameters()
        {
            // arrange
            Template template = new Template(Guid.NewGuid().ToString("N"),
                Guid.NewGuid().ToString("N"),
                Enumerable.Empty<WriteMethod>(),
                Enumerable.Empty<NamespaceModel>(),
                5, false, null);

            EventSourceModel eventSourceModel = new EventSourceModel();
            EventModel eventModel = new EventModel();
            eventModel.Attribute.Properties.Add(new AttributePropertyModel
            {
                Value = "1"
            });
            eventModel.Attribute.Properties.Add(new AttributePropertyModel
            {
                Name = "Message",
                Value = "{foo} {bar}"
            });
            eventModel.InputParameters.Add(new EventParameterModel
            {
                Name = "foo",
                Type = "string"
            });
            eventModel.InputParameters.Add(new EventParameterModel
            {
                Name = "bar",
                Type = "int"
            });
            eventSourceModel.Events.Add(eventModel);

            // act
            EventSourceModelPostProcessor postProcessor
                = new EventSourceModelPostProcessor(template);
            postProcessor.Process(eventSourceModel);

            // assert
            Assert.Equal("{5} {6}",
                eventModel.Attribute.Properties.Skip(1).First().Value);
        }

        [Fact]
        public void CheckComplexParameter()
        {
            // arrange
            Template template = new Template(Guid.NewGuid().ToString("N"),
                Guid.NewGuid().ToString("N"), Enumerable.Empty<WriteMethod>(), Enumerable.Empty<NamespaceModel>(), 5, true, "attachmentId");

            EventSourceModel eventSourceModel = new EventSourceModel();
            EventModel eventModel = new EventModel();
            eventModel.InputParameters.Add(new EventParameterModel
            {
                Type = "string"
            });
            eventModel.InputParameters.Add(new EventParameterModel
            {
                Type = "int"
            });
            eventModel.InputParameters.Add(new EventParameterModel
            {
                Type = "Exception"
            });
            eventSourceModel.Events.Add(eventModel);

            // act
            EventSourceModelPostProcessor postProcessor
                = new EventSourceModelPostProcessor(template);
            postProcessor.Process(eventSourceModel);

            // assert
            eventSourceModel.WriteMethods.Should().HaveCount(1);

            WriteCoreModel writeCoreModel = eventSourceModel.WriteMethods.First();
            writeCoreModel.Parameters.Should().HaveCount(3);
            writeCoreModel.Parameters.First().Position.Should().Be(5);
            writeCoreModel.Parameters.First().Type.Should().Be("string");
            writeCoreModel.Parameters.ElementAt(1).Position.Should().Be(6);
            writeCoreModel.Parameters.ElementAt(1).Type.Should().Be("string");
            writeCoreModel.Parameters.Last().Position.Should().Be(7);
            writeCoreModel.Parameters.Last().Type.Should().Be("int");
        }

        [Fact]
        public void CheckMultipleComplexParameters()
        {
            // arrange
            Template template = new Template(Guid.NewGuid().ToString("N"),
                Guid.NewGuid().ToString("N"), Enumerable.Empty<WriteMethod>(), Enumerable.Empty<NamespaceModel>(), 5, true, "attachmentId");

            EventSourceModel eventSourceModel = new EventSourceModel();
            EventModel eventModel = new EventModel();
            eventModel.InputParameters.Add(new EventParameterModel
            {
                Type = "string"
            });
            eventModel.InputParameters.Add(new EventParameterModel
            {
                Type = "int"
            });
            eventModel.InputParameters.Add(new EventParameterModel
            {
                Type = "Exception"
            });
            eventModel.InputParameters.Add(new EventParameterModel
            {
                Type = "ContractInfo"
            });
            eventSourceModel.Events.Add(eventModel);

            // act
            EventSourceModelPostProcessor postProcessor
                = new EventSourceModelPostProcessor(template);
            postProcessor.Process(eventSourceModel);

            // assert
            eventSourceModel.WriteMethods.Should().HaveCount(1);

            WriteCoreModel writeCoreModel = eventSourceModel.WriteMethods.First();
            writeCoreModel.Parameters.Should().HaveCount(3);
            writeCoreModel.Parameters.First().Position.Should().Be(5);
            writeCoreModel.Parameters.First().Type.Should().Be("string");
            writeCoreModel.Parameters.ElementAt(1).Position.Should().Be(6);
            writeCoreModel.Parameters.ElementAt(1).Type.Should().Be("string");
            writeCoreModel.Parameters.Last().Position.Should().Be(7);
            writeCoreModel.Parameters.Last().Type.Should().Be("int");
        }

        [Fact]
        public void DoNotAddTemplateWriteMethods()
        {
            // arrange
            Template template = new Template(Guid.NewGuid().ToString("N"),
                Guid.NewGuid().ToString("N"), new[] { new WriteMethod(new[] { "string" }) }, Enumerable.Empty<NamespaceModel>(), 5, false, null);

            EventSourceModel eventSourceModel = new EventSourceModel();
            EventModel eventModel = new EventModel();
            eventModel.InputParameters.Add(new EventParameterModel
            {
                Type = "string"
            });
            eventModel.InputParameters.Add(new EventParameterModel
            {
                Type = "int"
            });
            eventSourceModel.Events.Add(eventModel);

            eventModel = new EventModel();
            eventModel.InputParameters.Add(new EventParameterModel
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
