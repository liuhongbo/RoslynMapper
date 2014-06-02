using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoslynMapper.Map;

namespace RoslynMapper.Mapper
{
    public abstract class MapperBase<T1,T2>
    {
        public abstract ITypeMap<T1,T2> TypeMap
        {
            get;
        }

        public abstract IMapEngine MapEngine
        {
            get;
        }

        public void MemberResolve(MemberKey key, T1 t1, T2 t2)
        {
            var mapKey = new MapKey(typeof(T1), typeof(T2), TypeMap.Name);

        }
    }
}
