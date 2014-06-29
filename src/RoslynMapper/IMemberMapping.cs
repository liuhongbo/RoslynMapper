using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RoslynMapper
{
    public interface IMemberMapping
    {
        void Ignore();
    }

    public interface IMemberMapping<T1, T2> : IMemberMapping
    {        
        void Bind(Expression<Func<T1, object>> t1);
        void Resolve(Action<T1, T2> resolver);
        void CodeResolve(Func<string> resolver);
    }
}
