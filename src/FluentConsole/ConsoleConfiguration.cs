using System;

namespace ChilliCream.FluentConsole
{
    public class ConsoleConfiguration
        : CommandLineTaskConfiguration
    {
        public void AddTaskConfiguration<TConfiguration>(TConfiguration configuration)
            where TConfiguration : CommandLineTaskConfiguration
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            foreach (TaskDefinition taskDefinition in configuration.GetTaskDefinitions())
            {
                AddTaskDefinition(taskDefinition);
            }
        }

        public void AddTaskConfiguration<TConfiguration>()
            where TConfiguration : CommandLineTaskConfiguration, new()
        {
            AddTaskConfiguration(new TConfiguration());
        }

        public ConsoleRuntime CreateRuntime()
        {
            return new ConsoleRuntime(ClassicConsole.Default, GetTaskDefinitions());
        }

        public ConsoleRuntime CreateRuntime(IConsole console)
        {
            return new ConsoleRuntime(console, GetTaskDefinitions());
        }
    }
}
