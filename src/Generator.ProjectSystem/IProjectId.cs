using System;

namespace ChilliCream.Tracing.Generator.ProjectSystem
{
    /// <summary>
    /// Implement this to provide a typed project 
    /// identifier that is solution unique.
    /// Override <see cref="object.GetHashCode()"/> 
    /// and <see cref="object.Equals(object)"/> in 
    /// <see cref="IProjectId"/> implementations.
    /// </summary>
    /// <seealso cref="System.IEquatable{IProjectId}" />
    public interface IProjectId
        : IEquatable<IProjectId>
    {
    }

}
