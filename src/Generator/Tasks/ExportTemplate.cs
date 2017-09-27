using System;
using System.Collections.Generic;
using System.Text;
using ChilliCream.FluentConsole;

namespace ChilliCream.Tracing.Generator.Tasks
{
    public sealed class ExportTemplate
        : ITask
    {
        public string FileName { get; set; }
        public string Language { get; set; }
        public string Name { get; set; }

        public void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
