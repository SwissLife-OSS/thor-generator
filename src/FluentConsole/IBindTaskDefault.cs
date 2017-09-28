namespace ChilliCream.FluentConsole
{
    public interface IBindTaskDefault<TTask>
        : IArgumentsBindable<TTask>
        where TTask : class, ICommandLineTask
    {
        IBindTaskName<TTask> WithName(string name);
        IBindTaskName<TTask> WithName(params string[] name);
    }
}
