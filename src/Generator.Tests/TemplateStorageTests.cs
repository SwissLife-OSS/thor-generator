using System;
using System.IO;
using System.Linq;
using Thor.Generator.Templates;
using FluentAssertions;
using Xunit;

namespace Thor.Generator
{
    public class TemplateStorageTests
        : IDisposable
    {
        private readonly string _testAppDirectory;
        private readonly TemplateStorage _templateStorage;

        public TemplateStorageTests()
        {
            _testAppDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
            _templateStorage = new TemplateStorage(_testAppDirectory);

            Directory.CreateDirectory(_testAppDirectory);
        }

        [Fact]
        public void AddCustomTemplate()
        {
            // arrange
            string templateName = Guid.NewGuid().ToString("N");
            _templateStorage.CustomTemplateExists(templateName)
                .Should().BeFalse("the template should not exist");

            // act
            Template template = new Template(templateName,
                Guid.NewGuid().ToString("N"),
                Enumerable.Empty<WriteMethod>(),
                Enumerable.Empty<NamespaceModel>(), 0, 
                false, null);
            _templateStorage.SaveCustomTemplate(template);

            // assert
            _templateStorage.CustomTemplateExists(templateName)
                .Should().BeTrue("because we just created the template");

            Template retrievedTemplate = _templateStorage.GetCustomTemplate(templateName);
            retrievedTemplate.Should().NotBeNull();
            retrievedTemplate.Name.Should().Be(templateName);
            retrievedTemplate.Code.Should().Be(template.Code);
            retrievedTemplate.DefaultPayloads.Should().Be(template.DefaultPayloads);
            retrievedTemplate.BaseWriteMethods.Should().BeEmpty();
        }

        [Fact]
        public void AddCustomTemplateWithBaseWriteMethods()
        {
            // arrange
            string templateName = Guid.NewGuid().ToString("N");
            _templateStorage.CustomTemplateExists(templateName)
                .Should().BeFalse("the template should not exist");

            // act
            Template template = new Template(templateName,
                Guid.NewGuid().ToString("N"),
                new[] { new WriteMethod(new[] { "string" }) },
                Enumerable.Empty<NamespaceModel>(), 5, 
                false, null);
            _templateStorage.SaveCustomTemplate(template);

            // assert
            _templateStorage.CustomTemplateExists(templateName)
                .Should().BeTrue("because we just created the template");

            Template retrievedTemplate = _templateStorage.GetCustomTemplate(templateName);
            retrievedTemplate.Should().NotBeNull();
            retrievedTemplate.Name.Should().Be(templateName);
            retrievedTemplate.Code.Should().Be(template.Code);
            retrievedTemplate.DefaultPayloads.Should().Be(template.DefaultPayloads);
            retrievedTemplate.BaseWriteMethods.Should().HaveCount(1);

            WriteMethod writeMethod = retrievedTemplate.BaseWriteMethods.First();
            writeMethod.ParameterTypes.Should().HaveCount(1);
            writeMethod.ParameterTypes.First().Should().Be("string");
        }

        [Fact]
        public void CustomTemplateExistsArgumentValidation()
        {
            // act
            Action a = () => _templateStorage.CustomTemplateExists(null);

            // assert
            a.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void GetCustomTemplateArgumentValidation()
        {
            // act
            Action a = () => _templateStorage.GetCustomTemplate(null);
            Action b = () => _templateStorage.GetCustomTemplate("foo");

            // assert
            a.ShouldThrow<ArgumentNullException>();
            b.ShouldThrow<FileNotFoundException>();
        }

        [Fact]
        public void SaveCustomTemplateArgumentValidation()
        {
            // act
            Action a = () => _templateStorage.SaveCustomTemplate(null);

            // assert
            a.ShouldThrow<ArgumentNullException>();
        }

        public void Dispose()
        {
            foreach (string file in Directory.GetFiles(_testAppDirectory,
                "*.*", SearchOption.AllDirectories))
            {
                try
                {
                    File.Delete(file);
                }
                catch
                {
                    // ignore
                }
            }

            Directory.Delete(_testAppDirectory, true);
        }
    }
}
