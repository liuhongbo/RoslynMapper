using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RoslynMapper
{
    public interface IMapEngine
    {
        IMapper<T1, T2> GetMapper<T1, T2>();
        IMapper<T1, T2> GetMapper<T1, T2>(string name);

        IMapping<T1, T2> SetMapper<T1, T2>();
        IMapping<T1, T2> SetMapper<T1, T2>(string name);
       
        bool Build();

        T2 Map<T1,T2>(T1 t1);
        T2 Map<T1,T2>(T1 t1, T2 t2);

        T2 Map<T1, T2>(string name, T1 t1);
        T2 Map<T1, T2>(string name, T1 t1, T2 t2);

        IBuilder Builder { get; }        

        //T2 Map<T2>(object t1);
        //T2 Map<T2>(object t1, object t2);
        //object Map(object t1, object t2);
    }
}
