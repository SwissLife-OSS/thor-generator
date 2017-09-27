using System;
using System.IO;
using System.Text;
using ChilliCream.Tracing.Generator.ProjectSystem;
using ChilliCream.Tracing.Generator.Resources;

namespace ChilliCream.Tracing.Generator
{
    internal static class TemplateResolver
    {
        public static string Resolve(Language language)
        {
            switch (language)
            {
                case Language.CSharp:
                    return Encoding.UTF8.GetString(Templates.EventSourceBase);
                default:
                    throw new NotSupportedException($"{language} is not supported as template language.");
            }
        }

        public static string Resolve(string templateDirectory, string name, Language language)
        {
            string templateContent = null;
            if (string.IsNullOrEmpty(name))
            {
                templateContent = Resolve(language);
            }
            else
            {
                string templateFileName = CreateTemplateFileName(templateDirectory, name);
                if (File.Exists(templateFileName))
                {
                    templateContent = File.ReadAllText(templateFileName);
                }
            }
            return templateContent;
        }

        public static string CreateTemplateFileName(string templateDirectory, string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            return Path.Combine(templateDirectory,
                name.ToLowerInvariant() + ".mustache");
        }
    }
}
