using System;
using System.Collections.Generic;

namespace ChilliCream.FluentConsole
{
    internal static class ArgumentParser
    {
        public static IEnumerable<Argument> Parse(string[] args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            bool isPositional = true;
            int position = 0;

            Argument argument = null;
            foreach (string arg in args)
            {
                if (arg[0] == '-')
                {
                    if (argument != null)
                    {
                        yield return argument;
                    }

                    isPositional = false;

                    string name = arg.StartsWith("--")
                        ? arg.Substring(2)
                        : arg.Substring(1);

                    bool isSelected = !(arg[arg.Length - 1] == '-');

                    if (!isSelected)
                    {
                        name = name.Substring(0, name.Length - 1);
                    }

                    argument = new Argument
                    {
                        Name = name,
                        Position = position++,
                        IsSelected = isSelected,
                        Value = string.Empty
                    };
                }
                else if (isPositional)
                {
                    if (argument != null)
                    {
                        yield return argument;
                    }

                    argument = new Argument
                    {
                        Value = arg,
                        Position = position++
                    };
                }
                else if (!isPositional)
                {
                    if (!string.IsNullOrEmpty(argument.Value))
                    {
                        argument.Value += " ";
                    }
                    argument.Value += arg;
                }
            }
            yield return argument;
        }
    }
}
