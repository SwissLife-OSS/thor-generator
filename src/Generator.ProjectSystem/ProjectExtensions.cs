using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace ChilliCream.Tracing.Generator.ProjectSystem
{
    public static class ProjectExtensions
    {
        public static Document UpdateDocument(this Project project, string content, string name, params string[] folders)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            if (string.IsNullOrWhiteSpace(content))
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (folders == null)
            {
                throw new ArgumentNullException(nameof(folders));
            }

            return project.UpdateDocument(content, name, folders);
        }
    }
}
