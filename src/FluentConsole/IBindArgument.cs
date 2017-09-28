namespace ChilliCream.FluentConsole
{
    public interface IBindArgument<TTask>
        : IFluent
        where TTask : class, ICommandLineTask
    {
        IBindArgument<TTask> WithName(string name);
        IBindArgument<TTask> WithName(string name, char key);
        IBindArgument<TTask> Position(int position);
        IBindArgument<TTask> Mandatory();
        IBindAdditionalArgument<TTask> And();
    }
}
