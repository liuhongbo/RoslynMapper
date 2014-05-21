using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RoslynMapper
{
    public interface IMapping
    {
    }

    public interface IMapping<T1, T2> : IMapping
    {
        IMapping<T1, T2> Ignore(Expression<Func<T2, object>> destinationMember);
    }
}
