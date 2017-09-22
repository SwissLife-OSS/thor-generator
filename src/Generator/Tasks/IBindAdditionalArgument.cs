namespace ChilliCream.Tracing.Generator.Tasks
{
    public interface IBindAdditionalArgument<TTask>
        : IArgumentsBindable<TTask>
        where TTask : class, ITask
    {

    }
}
