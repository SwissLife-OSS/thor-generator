using System;
using System.Linq.Expressions;

namespace ChilliCream.Tracing.Generator.Tasks
{
    public interface IArgumentsBindable<TTask>
        : IFluent
        where TTask : class, ITask
    {
        IBindArgument<TTask> Argument(Expression<Func<TTask, object>> property);
    }
}
