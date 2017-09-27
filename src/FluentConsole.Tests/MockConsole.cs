using System;
using System.Collections.Generic;
using System.Text;

namespace ChilliCream.FluentConsole
{
    public class MockConsole
        : IConsole
    {
        public readonly List<string> Messages = new List<string>();

        public void Write(string s)
        {
            Messages.Add(s);
        }
    }
}
