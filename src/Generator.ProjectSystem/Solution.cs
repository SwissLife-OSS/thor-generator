using System;
using System.Collections.Generic;
using Thor.Generator.ProjectSystem.CSharp;
using Microsoft.Build.Construction;

namespace Thor.Generator.ProjectSystem
{
    /// <summary>
    /// Represents a collection of .net projects.
    /// </summary>
    public sealed class Solution
    {
        private HashSet<Project> _projects;

        private Solution(IEnumerable<Project> projects)
        {
            _projects = new HashSet<Project>(projects);
        }

        /// <summary>
        /// Gets the projects of this solution.
        /// </summary>
        /// <value>The projects.</value>
        public IReadOnlyCollection<Project> Projects => _projects;

        /// <summary>
        /// Commits changes to the projects of this solution.
        /// </summary>
        public void CommitChanges()
        {
            foreach (Project project in _projects)
            {
                CSharpProjectSystems.TryCommitChanges(project);
            }
        }

        /// <summary>
        /// Creates a new solution object.
        /// </summary>
        /// <param name="projects">The projects.</param>
        /// <returns>
        /// Returns a new solution object.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="projects"/> is <c>null</c>.
        /// </exception>
        public static Solution Create(IEnumerable<Project> projects)
        {
            if (projects == null)
            {
                throw new ArgumentNullException(nameof(projects));
            }

            return new Solution(projects);
        }

        /// <summary>
        /// Creates a new solution object from a solution file.
        /// </summary>
        /// <param name="solutionFileName">Name of the solution file.</param>
        /// <returns>Returns a new solution object from a solution file.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="solutionFileName"/> is <c>null</c>.
        /// </exception>
        public static Solution Create(string solutionFileName)
        {
            if (solutionFileName == null)
            {
                throw new ArgumentNullException(nameof(solutionFileName));
            }

            List<Project> projects = new List<Project>();
            SolutionFile solutionFile = SolutionFile.Parse(solutionFileName);
            foreach (ProjectInSolution project in solutionFile.ProjectsInOrder)
            {
                if (project.ProjectType == SolutionProjectType.KnownToBeMSBuildFormat)
                {
                    if (CSharpProjectSystems.TryOpenProject(project.AbsolutePath,
                        out Project p))
                    {
                        projects.Add(p);
                    }
                }
            }
            return new Solution(projects);
        }
    }
}
