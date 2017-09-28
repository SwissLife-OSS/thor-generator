using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Moq;
using Xunit;

namespace ChilliCream.Tracing.Generator.ProjectSystem.Tests
{
    public class SolutionTests
    {
        [Fact]
        public void SolutionCreate()
        {
            // arrange
            Mock<IProjectId> projectId = new Mock<IProjectId>(MockBehavior.Strict);
            Document document = Document.Create(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            Project project = Project.Create(projectId.Object, new[] { document });

            // act
            Solution solution = Solution.Create(new[] { project });

            // assert
            solution.Projects
                .Should().NotBeNull()
                .And.HaveCount(1);

            solution.Projects
                .First()
                .Should()
                .Be(project);
        }

        [Fact]
        public void SolutionCreateArgumentValidation()
        {
            // arrange
            Mock<IProjectId> projectId = new Mock<IProjectId>(MockBehavior.Strict);
            Document document = Document.Create(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            Project project = Project.Create(projectId.Object, new[] { document });

            // act
            Action a = () => Solution.Create(new[] { project });
            Action b = () => Solution.Create(Enumerable.Empty<Project>());
            Action c = () => Solution.Create((IEnumerable<Project>)null);
            Action d = () => Solution.Create((string)null);

            // assert
            a.ShouldNotThrow();
            b.ShouldNotThrow();
            c.ShouldThrow<ArgumentNullException>();
            d.ShouldThrow<ArgumentNullException>();
        }
    }
}
