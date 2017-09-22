namespace ChilliCream.Tracing.Generator.Tasks
{
    public interface IBindTaskDefault<TTask>
        : IArgumentsBindable<TTask>
        where TTask : class, ITask
    {
        IBindTaskName<TTask> WithName(string name);
    }
}
