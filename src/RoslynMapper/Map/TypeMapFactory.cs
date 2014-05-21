using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynMapper.Map
{
    public class TypeMapFactory: ITypeMapFactory
    {
        public ITypeMap<T1,T2> CreateTypeMap<T1,T2>()
        {
            return CreateTypeMap<T1,T2>(null);
        }
        public ITypeMap<T1,T2> CreateTypeMap<T1,T2>(string name)
        {
            return new TypeMap<T1, T2>(name);
        }
    }
}
