using System.Collections.Generic;

namespace ChilliCream.FluentConsole
{
    public class MockConsole
        : IConsole
    {
        public readonly List<string> Messages = new List<string>();
        public readonly List<string> Errors = new List<string>();

        public void Error(string message)
        {
            Errors.Add(message);
        }

        public string GetFullPath(string fileOrDirectory)
        {
            return fileOrDirectory;
        }

        public void Write(string value)
        {
            Messages.Add(value);
        }

        public void WriteLine(string value)
        {
            Messages.Add(value);
        }
    }
}
