using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ChilliCream.Logging.Generator.Templates;
using ChilliCream.Tracing.Generator.Properties;
using Newtonsoft.Json;

namespace ChilliCream.Tracing.Generator.Templates
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
        /// Initializes a new instance of the <see cref="Template"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="code">The code.</param>
        /// <param name="baseWriteMethods">The base write methods.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name"/> is <c>null</c>
        /// or
        /// <paramref name="name"/> is <see cref="string.Empty"/>
        /// or
        /// <paramref name="code"/> is <c>null</c>
        /// or
        /// <paramref name="code"/> is <see cref="string.Empty"/>
        /// or
        /// <paramref name="baseWriteMethods"/> is <c>null</c>.
        /// baseWriteMethods
        /// </exception>
        public Template(string name, string code, IEnumerable<WriteMethod> baseWriteMethods)
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
                    .ToList()
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

            return new Template(name, File.ReadAllText(fileName),
                templateInfo.BaseWriteMethods.Select(t => new WriteMethod(t)).Distinct());
        }

        private class TemplateInfo
        {
            public List<string[]> BaseWriteMethods { get; set; }
        }
    }
}
