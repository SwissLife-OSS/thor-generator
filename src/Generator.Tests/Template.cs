using System;
using System.Diagnostics.Tracing;
using System.Text;

namespace TemplateNamespace
{
    [EventSource]
    public sealed class Template
        : EventSource
    {
        private Template() { }

        public static readonly Template Log = new Template();

    }
}
