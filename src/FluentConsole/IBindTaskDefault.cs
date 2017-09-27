namespace ChilliCream.FluentConsole
{
    public interface IBindTaskDefault<TTask>
        : IArgumentsBindable<TTask>
        where TTask : class, ITask
    {
        IBindTaskName<TTask> WithName(string name);
        IBindTaskName<TTask> WithName(params string[] name);
    }
}
