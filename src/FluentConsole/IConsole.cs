namespace ChilliCream.FluentConsole
{
    public interface IConsole
    {
        void Write(string s);

        string GetFullPath(string fileOrDirectory);

        void Error(string message);
    }
}
