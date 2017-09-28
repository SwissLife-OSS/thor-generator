namespace ChilliCream.FluentConsole
{
    public interface IConsole
    {
        void Write(string value);

        void WriteLine(string value);

        string GetFullPath(string fileOrDirectory);

        void Error(string message);
    }
}
