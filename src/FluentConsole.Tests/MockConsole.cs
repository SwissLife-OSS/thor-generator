using System.Collections.Generic;

namespace ChilliCream.FluentConsole
{
    public class MockConsole
        : IConsole
    {
        public readonly List<string> Messages = new List<string>();

        public string GetFullPath(string fileOrDirectory)
        {
            return fileOrDirectory;
        }

        public void Write(string s)
        {
            Messages.Add(s);
        }
    }
}
