using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ChilliCream.FluentConsole
{
    public class ClassicConsole
        : IConsole
    {
        private ConsoleColor _default = Console.ForegroundColor;
        private ConsoleColor _error = ConsoleColor.Red;

        private ClassicConsole()
        {

        }

        public static readonly ClassicConsole Default = new ClassicConsole();

        public void Write(string value)
        {
            Console.ForegroundColor = _default;
            Console.Write(value);
        }

        public void WriteLine(string value)
        {
            Console.ForegroundColor = _default;
            Console.WriteLine(value);
        }

        public void Error(string message)
        {
            Console.ForegroundColor = _error;
            Console.WriteLine(message);
            Console.ForegroundColor = _default;
        }

        public string GetFullPath(string fileOrDirectoryName)
        {
            if (Path.IsPathRooted(fileOrDirectoryName))
            {
                return fileOrDirectoryName;
            }

            return Path.GetFullPath(Path.Combine(
                Environment.CurrentDirectory, fileOrDirectoryName));
        }
    }
}
