using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoslynMapper.Map;

namespace RoslynMapper
{
    public interface IMapper
    {
        //object Map(Object from, Type to);
    }

    public interface IMapper<T1, T2> : IMapper
    {
        T2 Map(T1 t1);
        T2 Map(T1 t1, T2 t2);
    }
}
