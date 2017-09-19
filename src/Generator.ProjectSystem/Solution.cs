using System;
using System.Collections.Generic;

namespace ChilliCream.Tracing.Generator.ProjectSystem
{
    /// <summary>
    /// Represents a collection of .net projects.
    /// </summary>
    public class Solution
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
        /// Creates a new solution object.
        /// </summary>
        /// <param name="projects">The projects.</param>
        /// <returns>
        /// Returns a new solution object.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="projects"/> is <c>null</c>.
        /// </exception>
        public Solution Create(IEnumerable<Project> projects)
        {
            if (projects == null)
            {
                throw new ArgumentNullException(nameof(projects));
            }

            return new Solution(projects);
        }
    }
}
