using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RoslynMapper
{
    // the fluent mapping configuration interface
    public interface IMapping
    {
       
    }

    public interface IMapping<T1, T2> : IMapping
    {
        IMapping<T1, T2> For(Expression<Func<T2, object>> t2, Action<IMemberMapping<T1, T2>> mapping);
        IMapping<T1, T2> For(Expression<Func<T1, object>> t1, Action<IMemberMapping<T1, T2>> mapping);
        IMapping<T1, T2> Ignore(Expression<Func<T2, object>> t2);
        IMapping<T1, T2> Ignore(Expression<Func<T1, object>> t1);
        IMapping<T1, T2> Bind(Expression<Func<T1, object>> t1, Expression<Func<T2, object>> t2);
        IMapping<T1, T2> Resolve(Expression<Func<T2, object>> t2, Action<T1, T2> resolver);
    }
}
