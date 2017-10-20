using System.Collections.Generic;

namespace ChilliCream.Tracing.Generator.Templates
{
    /// <summary>
    /// Represents an eventsource template model for the write methods.
    /// </summary>
    internal class WriteCoreModel
    {
        /// <summary>
        /// Gets or sets the total parameters count.
        /// </summary>
        /// <value>The total parameters count.</value>
        public int TotalParameters { get; set; }

        /// <summary>
        /// Gets or sets the write method parameters.
        /// </summary>
        /// <value>The parameters.</value>
        public List<WriteMethodParameterModel> Parameters { get; set; } = new List<WriteMethodParameterModel>();
    }
}
