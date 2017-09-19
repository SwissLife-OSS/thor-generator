using System.Collections.Generic;

namespace ChilliCream.Tracing.Generator.ProjectSystem
{
    public class Solution
    {
        public HashSet<Project> _projects;

        private Solution(IEnumerable<Project> projects)
        {
            _projects = new HashSet<Project>(projects);
        }

        public IReadOnlyCollection<Project> Projects => _projects;

        public Solution Create(IEnumerable<Project> projects)
        {
            if (projects == null)
            {
                throw new System.ArgumentNullException(nameof(projects));
            }

            return new Solution(projects);
        }
    }
}
