using System;
using System.Linq.Expressions;

namespace ChilliCream.Tracing.Generator.Tasks
{
    public interface IArgumentsBindable<TTask>
        : IFluent
        where TTask : class, ITask
    {
        IBindArgument<TTask> Argument<TProperty>(Expression<Func<TTask, TProperty>> property);
    }
}
