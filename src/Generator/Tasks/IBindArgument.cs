namespace ChilliCream.Tracing.Generator.Tasks
{
    public interface IBindArgument<TTask>
        : IFluent
        where TTask : class, ITask
    {
        IBindArgument<TTask> WithName(string name);
        IBindArgument<TTask> WithName(string name, char key);
        IBindArgument<TTask> Position(int porition);
        IBindArgument<TTask> Mandatory();
        IBindAdditionalArgument<TTask> And();
    }
}
