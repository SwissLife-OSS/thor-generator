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
            ReferenceEquals(project.Documents.First(), document).Should().BeTrue();
        }

        [Fact]
        public void ProjectGetDocument()
        {
            // arrange
            Mock<IProjectId> projectId = new Mock<IProjectId>(MockBehavior.Strict);
            Document a = Document.Create(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            Document b = Document.Create(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            Document c = Document.Create(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            Project project = Project.Create(projectId.Object, new[] { a, b });

            // act
            Document retrieved = project.GetDocument(a.Id);
            Document doesnotexist = project.GetDocument(c.Id);

            // assert
            retrieved.Should().Be(a);
            doesnotexist.Should().BeNull();
        }


        [Fact]
        public void ProjectGetDocumentArgumentValidation()
        {
            // arrange
            Mock<IProjectId> projectId = new Mock<IProjectId>(MockBehavior.Strict);
            Document a = Document.Create(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            Document b = Document.Create(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            Project project = Project.Create(projectId.Object, new[] { a, b });

            // act
            Action getDocument = () => project.GetDocument(null);

            // assert
            getDocument.ShouldThrow<ArgumentNullException>();
        }
    }
}
