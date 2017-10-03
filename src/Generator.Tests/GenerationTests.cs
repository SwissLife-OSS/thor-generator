using System;
using System.Collections.Generic;
using System.Text;
using ChilliCream.Tracing.Generator.Properties;
using Nustache.Core;
using Xunit;

namespace ChilliCream.Tracing.Generator
{
    public class GenerationTests
    {
        [Fact]
        public void FooTest()
        {
            var x = Render.StringToString(Resources.fooTemplate, new Foo (),
                renderContextBehaviour: new RenderContextBehaviour { HtmlEncoder = t => t });
        }

    }

    public class  Foo
    {
        public bool IsTrue { get; set; } = true;
    }

}
