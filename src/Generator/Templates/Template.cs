using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Thor.Generator.Properties;
using Newtonsoft.Json;

namespace Thor.Generator.Templates
{
    /// <summary>
    /// Represents an event source template.
    /// </summary>
    public class Template
    {
        /// <summary>
        /// The template file extension
        /// </summary>
        public static readonly string TemplateExtension = ".mustache";

        /// <summary>
        /// The template info file extension
        /// </summary>
        public static readonly string TemplateInfoExtension = ".json";

        /// <summary>
        /// Initializes a new instance of the <see cref="Template" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="code">The code.</param>
        /// <param name="baseWriteMethods">The base write methods.</param>
        /// <param name="usings">The usings.</param>
        /// <param name="defaultPayloads">The payloads that come from the context.</param>
        /// <param name="allowComplexParameters">if set to <c>true</c> [allow large payloads].</param>
        /// <param name="eventComplexParameterName">Name of the event complex parameter.</param>
        /// <exception cref="ArgumentNullException"><paramref name="name" /> is <c>null</c>
        /// or
        /// <paramref name="name" /> is <see cref="string.Empty" />
        /// or
        /// <paramref name="code" /> is <c>null</c>
        /// or
        /// <paramref name="code" /> is <see cref="string.Empty" />
        /// or
        /// <paramref name="baseWriteMethods" /> is <c>null</c>.</exception>
        public Template(string name, string code,
            IEnumerable<WriteMethod> baseWriteMethods,
            IEnumerable<NamespaceModel> usings,
            int defaultPayloads,
            bool allowComplexParameters,
            string eventComplexParameterName)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentNullException(nameof(code));
            }

            if (baseWriteMethods == null)
            {
                throw new ArgumentNullException(nameof(baseWriteMethods));
            }

            Name = name;
            Code = code;
            BaseWriteMethods = baseWriteMethods.ToArray();
            Usings = usings.ToArray();
            DefaultPayloads = defaultPayloads;
            AllowComplexParameters = allowComplexParameters;
            EventComplexParameterName = eventComplexParameterName;
        }

        /// <summary>
        /// Gets the name of the template.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; }

        /// <summary>
        /// Gets or sets the template code.
        /// </summary>
        /// <value>The code.</value>
        public string Code { get; }

        /// <summary>
        /// Gets the write methods that are provided by the templates event source base class.
        /// </summary>
        /// <value>The base write methods.</value>
        public IReadOnlyCollection<WriteMethod> BaseWriteMethods { get; }

        /// <summary>
        /// Gets the usings.
        /// </summary>
        /// <value>
        /// The usings.
        /// </value>
        public IReadOnlyCollection<NamespaceModel> Usings { get; }

        /// <summary>
        /// Gets the default payloads count which defines the additional payloads that are added from the context by this template.
        /// </summary>
        /// <value>The default payloads count.</value>
        public int DefaultPayloads { get; }

        /// <summary>
        /// Gets a value indicating whether the generator should allow large input parameters in the methods to generate.
        /// </summary>
        /// <value>
        /// AllowComplexParameters <c>true</c> if [allow large payloads]; otherwise, <c>false</c>.
        /// </value>
        public bool AllowComplexParameters { get; }

        /// <summary>
        /// Gets or sets the name of the event complex parameter.
        /// </summary>
        /// <value>
        /// The name of the event complex parameter.
        /// </value>
        public string EventComplexParameterName { get; set; }

        /// <summary>
        /// Saves this template to the specified directory.
        /// </summary>
        /// <param name="directoryName">Name of the template directory.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="directoryName"/> is <c>null</c>.
        /// </exception>
        public void Save(string directoryName)
        {
            if (directoryName == null)
            {
                throw new ArgumentNullException(nameof(directoryName));
            }

            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }

            string templateFile = Path.Combine(directoryName, Name + TemplateExtension);
            string templateInfoFile = Path.Combine(directoryName, Name + TemplateInfoExtension);

            TemplateInfo templateInfo = new TemplateInfo
            {
                BaseWriteMethods = BaseWriteMethods
                    .Select(t => t.ParameterTypes.ToArray())
                    .ToList(),
                Usings = Usings.ToList(),
                DefaultPayloads = DefaultPayloads
            };

            File.WriteAllText(templateFile, Code);
            File.WriteAllText(templateInfoFile, JsonConvert.SerializeObject(templateInfo));
        }

        /// <summary>
        /// Loads a template from a file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="name">The new name that the template shall have.</param>
        /// <returns>Returns a new template object.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="fileName"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="FileNotFoundException">
        /// <paramref name="fileName"/> does not exist.
        /// </exception>
        public static Template FromFile(string fileName, string name = null)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException(string.Format(Resources.TemplateFileNotExists, fileName), fileName);
            }

            string templateName = name ?? Path.GetFileNameWithoutExtension(fileName);
            string templateFileDirectory = Path.GetDirectoryName(fileName);
            string templateInfoFile = Path.Combine(templateFileDirectory,
                Path.GetFileNameWithoutExtension(fileName) + TemplateInfoExtension);

            TemplateInfo templateInfo = File.Exists(templateInfoFile)
                ? JsonConvert.DeserializeObject<TemplateInfo>(File.ReadAllText(templateInfoFile))
                : new TemplateInfo();

            if (templateInfo.BaseWriteMethods == null)
            {
                templateInfo.BaseWriteMethods = new List<string[]>();
            }

            return new Template(templateName, File.ReadAllText(fileName),
                templateInfo.BaseWriteMethods.Select(t => new WriteMethod(t)).Distinct(), templateInfo.Usings.Distinct(),
                templateInfo.DefaultPayloads,
                templateInfo.ComplexParameter?.Enabled ?? false,
                templateInfo.ComplexParameter?.EventParameterName);
        }

        private class TemplateInfo
        {
            public List<string[]> BaseWriteMethods { get; set; }
            public List<NamespaceModel> Usings { get; set; }
            public int DefaultPayloads { get; set; }
            public ComplexParameterInfo ComplexParameter { get; set; }
        }

        private class ComplexParameterInfo
        {
            public bool Enabled { get; set; }
            public string EventParameterName { get; set; }
        }
    }
}
