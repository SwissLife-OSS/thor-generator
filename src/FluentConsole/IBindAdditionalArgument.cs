namespace ChilliCream.FluentConsole
{
    public interface IBindAdditionalArgument<TTask>
        : IArgumentsBindable<TTask>
        where TTask : class, ITask
    {

    }
}
