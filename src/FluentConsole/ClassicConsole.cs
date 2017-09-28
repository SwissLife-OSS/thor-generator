using System;
using System.Collections.Generic;
using System.IO;
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
