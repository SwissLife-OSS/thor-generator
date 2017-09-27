using System;
using System.Collections.Generic;
using System.Text;

namespace ChilliCream.FluentConsole
{
    public sealed class ClassicConsole
        : IConsole
    {
        private ClassicConsole()
        {

        }

        public static readonly ClassicConsole Default = new ClassicConsole();

        public void Write(string s)
        {
        }
    }
}
