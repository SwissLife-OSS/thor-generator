using System;
using System.Linq.Expressions;

namespace ChilliCream.FluentConsole
{
    public interface IArgumentsBindable<TTask>
        : IFluent
        where TTask : class, ITask
    {
        IBindArgument<TTask> Argument<TProperty>(Expression<Func<TTask, TProperty>> property);
    }
}
