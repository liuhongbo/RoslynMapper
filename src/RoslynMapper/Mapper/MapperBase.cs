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

        public void Resolve(T1 t1, T2 t2)
        {
            if (TypeMap.Resolver != null)
            {
                TypeMap.Resolver(t1, t2);
            }
        }

        public void MemberResolve(string id, T1 t1, T2 t2)
        {
            IMember<T1,T2> member = TypeMap.Members.GetMember<T1, T2>(id);
            if (member != null)
            {
                if (member.Resolver != null)
                {
                    member.Resolver(t1, t2);
                }
            }
        }
    }
}
