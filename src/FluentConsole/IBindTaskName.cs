namespace ChilliCream.FluentConsole
{
    public interface IBindTaskName<TTask>
        : IArgumentsBindable<TTask>
        where TTask : class, ITask
    {
        IBindTaskDefault<TTask> AsDefault();
    }
}
