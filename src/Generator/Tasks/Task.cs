using System;
using System.Collections.Generic;
using System.Text;

namespace ChilliCream.Tracing.Generator.Tasks
{
    public class ConsoleRuntime
    {


        public void Run(string args)
        {

        }
    }
    

    public interface IConsole
    {

    }


    public static class Task
    {
        public static IBindTask<TTask> Bind<TTask>()
            where TTask : class, ITask
        {
            throw new NotImplementedException();
        }

    }
}
