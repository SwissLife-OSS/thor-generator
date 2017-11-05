using System;
using System.Linq;
using FluentAssertions;
using Moq;
using Xunit;

namespace Thor.Generator.ProjectSystem.Tests
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
        public void ProjectCreateArgumentValidation()
        {
            // arrange
            Mock<IProjectId> projectId = new Mock<IProjectId>(MockBehavior.Strict);
            Document document = Document.Create(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

            // act
            Action a = () => Project.Create(projectId.Object, new[] { document });
            Action b = () => Project.Create(null, new[] { document });
            Action c = () => Project.Create(projectId.Object, null);

            // assert
            a.ShouldNotThrow();
            b.ShouldThrow<ArgumentNullException>();
            c.ShouldThrow<ArgumentNullException>();
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
        public void ProjectUpdateDocumentArgumentValidation()
        {
            // arrange
            Mock<IProjectId> projectId = new Mock<IProjectId>(MockBehavior.Strict);
            Document originalDocument = Document.Create(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            Project project = Project.Create(projectId.Object, new[] { originalDocument });

            // act
            Action a = () => project.UpdateDocument(Guid.NewGuid().ToString(), originalDocument.Name, Enumerable.Empty<string>());
            Action b = () => ProjectExtensions.UpdateDocument(project, Guid.NewGuid().ToString(), originalDocument.Name, Array.Empty<string>());
            Action c = () => project.UpdateDocument(null, originalDocument.Name, Enumerable.Empty<string>());
            Action d = () => ProjectExtensions.UpdateDocument(project, null, originalDocument.Name, Array.Empty<string>());
            Action e = () => project.UpdateDocument(Guid.NewGuid().ToString(), null, Enumerable.Empty<string>());
            Action f = () => ProjectExtensions.UpdateDocument(project, Guid.NewGuid().ToString(), null, Array.Empty<string>());
            Action g = () => project.UpdateDocument(Guid.NewGuid().ToString(), originalDocument.Name, null);
            Action h = () => ProjectExtensions.UpdateDocument(project, Guid.NewGuid().ToString(), originalDocument.Name, null);
            Action i = () => ProjectExtensions.UpdateDocument(null, Guid.NewGuid().ToString(), originalDocument.Name, Array.Empty<string>());

            // assert
            a.ShouldNotThrow();
            b.ShouldNotThrow();
            c.ShouldThrow<ArgumentNullException>();
            d.ShouldThrow<ArgumentNullException>();
            e.ShouldThrow<ArgumentNullException>();
            f.ShouldThrow<ArgumentNullException>();
            g.ShouldThrow<ArgumentNullException>();
            h.ShouldThrow<ArgumentNullException>();
            i.ShouldThrow<ArgumentNullException>();
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
