using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Moq;
using Xunit;

namespace ChilliCream.Tracing.Generator.ProjectSystem.Tests
{
    public class ProjectTests
    {
        [Fact]
        public void ProjectCreate()
        {
            // arrange
            Mock<IProjectId> projectId = new Mock<IProjectId>(MockBehavior.Strict);
            Document document = Document.Create(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

            // act
            Project project = Project.Create(projectId.Object, new[] { document });

            // assert
            project.Id.Should().Be(projectId.Object);
            project.Documents.Should().HaveCount(1);
            project.UpdatedDocumets.Should().HaveCount(0);
            project.Documents.First().Should().Be(document);
        }

        [Fact]
        public void ProjectUpdateDocument()
        {
            // arrange
            Mock<IProjectId> projectId = new Mock<IProjectId>(MockBehavior.Strict);
            Document originalDocument = Document.Create(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            Project project = Project.Create(projectId.Object, new[] { originalDocument });

            // act
            Document document = project.UpdateDocument(Guid.NewGuid().ToString(), originalDocument.Name);

            // assert
            project.Id.Should().Be(projectId.Object);
            project.Documents.Should().HaveCount(1);
            project.UpdatedDocumets.Should().HaveCount(1);
            project.Documents.First().Should().Be(document);
        }

        
    }
}
