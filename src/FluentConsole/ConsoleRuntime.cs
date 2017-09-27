using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ChilliCream.FluentConsole
{
    public class ConsoleRuntime
    {
        private readonly List<TaskDefinition> _tasks = new List<TaskDefinition>();

        public IBindTask<TTask> Bind<TTask>()
            where TTask : class, ITask
        {
            return new BindTask<TTask>(_tasks);
        }

        public void Run(IConsole console, string[] args)
        {
            //Argument[] arguments = ArgumentParser.Parser(args).ToArray();
            //if (arguments.Any())
            //{
            //    if (TryResolveTask(arguments, out ResolvedTaskDefinition resolvedTaskDefinition))
            //    {
            //        //ITask task = CreateTask(resolvedTaskDefinition.TaskDefinition,
            //        //    arguments.Skip(resolvedTaskDefinition.ArgumentCount));
            //        //task.Execute();
            //    }
            //}
        }

        //private bool TryResolveTask(Argument[] arguments,
        //    out ResolvedTaskDefinition resolvedTaskDefinition)
        //{
        //    if (_tasks.Count == 0)
        //    {
        //        resolvedTaskDefinition = null;
        //        return false;
        //    }

        //    if (_tasks.Count == 1 && _tasks[0].IsDefault)
        //    {
        //        resolvedTaskDefinition = new ResolvedTaskDefinition(_tasks[0]);
        //        return true;
        //    }

        //    string name = string.Empty;
        //    ILookup<int, TaskDefinition> taskLookup = _tasks.ToLookup(t => t.Name.Length);
        //    int maxNameParts = _tasks.Select(t => t.Name?.Length ?? 0).Max();
        //    for (int i = 0; i < maxNameParts; i++)
        //    {
        //        if (string.IsNullOrEmpty(arguments[i].Name))
        //        {
        //            name += string.IsNullOrEmpty(name)
        //                ? arguments[i].Value
        //                : "-" + arguments[i].Value;
        //        }
        //        else
        //        {
        //            resolvedTaskDefinition = null;
        //            return false;
        //        }

        //        foreach (TaskDefinition taskDefinition in taskLookup[i + 1])
        //        {
        //            if (string.Equals(taskDefinition.Key, name,
        //                StringComparison.OrdinalIgnoreCase))
        //            {
        //                resolvedTaskDefinition = new ResolvedTaskDefinition(_tasks[0], i + 1);
        //                return true;
        //            }
        //        }
        //    }

        //    resolvedTaskDefinition = null;
        //    return false;
        //}

        //private bool TryCreateTask(IConsole console, TaskDefinition taskDefinition,
        //    IEnumerable<Argument> arguments, out ITask task)
        //{
        //    HashSet<ArgumentDefinition> argumentDefinitions =
        //        new HashSet<ArgumentDefinition>(taskDefinition
        //        .Arguments.Values.Concat(taskDefinition.PositionalArguments)
        //        .Distinct());
        //    HashSet<ArgumentDefinition> providedArguments = new HashSet<ArgumentDefinition>();

        //    if (TryCreateTask(console, taskDefinition.TaskType, out task))
        //    {
        //        foreach (Argument argument in arguments)
        //        {
        //            ArgumentDefinition argumentDefinition = null;
        //            if (string.IsNullOrEmpty(argument.Name))
        //            {
        //                argumentDefinition = argumentDefinitions
        //                    .FirstOrDefault(t => t.Position == argument.Position);
        //            }
        //            else
        //            {
        //                argumentDefinition = argumentDefinitions
        //                    .FirstOrDefault(t => t.Name.Equals(argument.Name,
        //                        StringComparison.OrdinalIgnoreCase));
        //            }

        //            if (argumentDefinition == null)
        //            {
        //                throw new InvalidOperationException("Could not resolve argument.");
        //            }

        //            providedArguments.Add(argumentDefinition);

        //            if (string.IsNullOrEmpty(argument.Value))
        //            {
        //                argumentDefinition.Property.SetValue(task, argument.IsSelected);
        //            }
        //            else
        //            {
        //                argumentDefinition.Property.SetValue(task, argument.Value);
        //            }
        //        }

        //        HashSet<ArgumentDefinition> mandatoryArguments =
        //            new HashSet<ArgumentDefinition>(argumentDefinitions.Where(t => t.IsMandatory));
        //        if (mandatoryArguments.Any() && mandatoryArguments.Except(providedArguments).Any())
        //        {

        //        }

        //        task = null;
        //        return false;
        //    }

        //    task = null;
        //    return false;
        //}

        //private bool TryCreateTask(IConsole console, Type taskType, out ITask task)
        //{
        //    task = null;

        //    ConstructorInfo constructor = taskType.GetConstructors().FirstOrDefault(t =>
        //    {
        //        ParameterInfo[] parameters = t.GetParameters();
        //        if (parameters.Length == 1 && parameters[0].ParameterType == typeof(IConsole))
        //        {
        //            return true;
        //        }
        //        return false;
        //    });

        //    if (constructor != null)
        //    {
        //        task = (ITask)constructor.Invoke(new object[] { console });
        //    }

        //    if (task == null)
        //    {
        //        constructor = taskType.GetConstructors().FirstOrDefault(t => t.GetParameters().Length == 0);
        //        task = (ITask)constructor.Invoke(Array.Empty<object>());
        //    }

        //    return task != null;
        //}

        //#region Nested Types

        
        //#endregion
    }
}
