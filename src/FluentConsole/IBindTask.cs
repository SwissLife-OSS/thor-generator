namespace ChilliCream.FluentConsole
{
    public interface IBindTask<TTask>
        : IFluent
        where TTask : class, ITask
    {
        IBindTaskName<TTask> WithName(string name);

        IBindTaskName<TTask> WithName(params string[] name);

        IBindTaskDefault<TTask> AsDefault();
    }
}
