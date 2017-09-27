using System;
using System.Collections.Generic;
using System.Text;

namespace ChilliCream.FluentConsole
{
    public static class Task
    {
        public static IBindTask<TTask> Bind<TTask>()
            where TTask : class, ITask
        {
            throw new NotImplementedException();
        }

    }
}
