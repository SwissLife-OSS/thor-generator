namespace ChilliCream.Tracing.Generator.Tasks
{
    public interface IBindTask<TTask>
        : IFluent
        where TTask : class, ITask
    {
        IBindTaskName<TTask> WithName(string name);
        IBindTaskDefault<TTask> AsDefault();
    }
}
