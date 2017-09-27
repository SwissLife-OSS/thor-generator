﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ChilliCream.Tracing.Generator.EventSourceDefinitions {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class EventSourceSamples {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal EventSourceSamples() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ChilliCream.Tracing.Generator.EventSourceDefinitions.EventSourceSamples", typeof(EventSourceSamples).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [&quot;a a&quot;,&quot;b&quot;,&quot;c&quot;,&quot;x&quot;,&quot;--zzz&quot;,&quot;-x&quot;,&quot;s&quot;,&quot;-y-&quot;,&quot;--aaa&quot;,&quot;s&quot;,&quot;s&quot;,&quot;-z&quot;,&quot;s s&quot;].
        /// </summary>
        internal static string Arguments {
            get {
                return ResourceManager.GetString("Arguments", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to using System;
        ///using System.Diagnostics.Tracing;
        ///using System.Text;
        ///
        ///namespace TemplateNamespace
        ///{
        ///    public sealed class Template
        ///        : EventSource
        ///    {
        ///        private Template() { }
        ///
        ///        public static readonly Template Log = new Template();
        ///    }
        ///}
        ///.
        /// </summary>
        internal static string EventSourceTemplate {
            get {
                return ResourceManager.GetString("EventSourceTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to using System;
        ///using System.Diagnostics.Tracing;
        ///
        ///namespace ManyArgumentsEventSourceNamespace
        ///{
        ///    [EventSourceDefinition(Name = &quot;ManyArgumentsEventSource&quot;)]
        ///    public interface IManyArgumentsEventSource
        ///    {
        ///        [Event(1, 
        ///            Level = EventLevel.Informational, 
        ///            Message = &quot;Simple is called {0}.&quot;, 
        ///            Version = 1)]
        ///        void Simple(string a, short b, int c, long d, ushort e, 
        ///            unit f, ulong g, decimal, System.Boolean h, bool i, double j);
        ///    }
        /// [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string ManyArgumentsEventSource {
            get {
                return ResourceManager.GetString("ManyArgumentsEventSource", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to using System;
        ///using System.Diagnostics.Tracing;
        ///
        ///namespace FooNamespace
        ///{
        ///    [EventSourceDefinition(Name = &quot;Mock&quot;)]
        ///    public interface ISimpleEventSource
        ///    {
        ///        [Event(1, 
        ///            Level = EventLevel.Informational, 
        ///            Message = &quot;Simple is called {0}.&quot;, 
        ///            Version = 2)]
        ///        void Simple(string name);
        ///    }
        ///}
        ///.
        /// </summary>
        internal static string SimpleEventSource {
            get {
                return ResourceManager.GetString("SimpleEventSource", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to using System;
        ///using System.Diagnostics.Tracing;
        ///
        ///namespace TwoEventsNamespace
        ///{
        ///    [EventSourceDefinition(Name = &quot;TwoEvents&quot;)]
        ///    public interface ITwoEventsEventSource
        ///    {
        ///        [Event(1, 
        ///            Level = EventLevel.Informational, 
        ///            Message = &quot;Simple is called {0}.&quot;, 
        ///            Version = 2)]
        ///        void A(string name);
        ///
        ///        [Event(2, 
        ///            Level = EventLevel.Informational, 
        ///            Message = &quot;Simple is called {0}.&quot;, 
        ///            Version = 2)]
        ///        [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string TwoEventsEventSource {
            get {
                return ResourceManager.GetString("TwoEventsEventSource", resourceCulture);
            }
        }
    }
}