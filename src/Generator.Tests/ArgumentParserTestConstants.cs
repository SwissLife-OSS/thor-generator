using System;
using System.Collections.Generic;
using System.Text;

namespace ChilliCream.Tracing.Generator
{
    public static class ArgumentParserTestConstants
    {
        public static readonly string[] DefaultArguments = new[]
        {
            "a a",
            "b",
            "c",
            "x",
            "--zzz",
            "-x",
            "s",
            "-y-",
            "-aaa",
            "s",
            "s",
            "-z",
            "s s"
        };
    }
}
