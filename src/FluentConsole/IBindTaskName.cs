namespace ChilliCream.FluentConsole
{
    public interface IBindTaskName<TTask>
        : IArgumentsBindable<TTask>
        where TTask : class, ICommandLineTask
    {
        IBindTaskDefault<TTask> AsDefault();
    }
}
