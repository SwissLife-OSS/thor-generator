namespace ChilliCream.FluentConsole
{
    public class CreateSolutionEventSources
       : ITask
    {
        public string FileOrDirectoryName { get; set; }
        public bool Recursive { get; set; }

        public void Execute()
        {
        }
    }
}
