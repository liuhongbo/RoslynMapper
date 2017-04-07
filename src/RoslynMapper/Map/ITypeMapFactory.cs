using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynMapper.Map
{
    public interface ITypeMapFactory
    {
        ITypeMap<T1,T2> CreateTypeMap<T1,T2>();
        ITypeMap<T1,T2> CreateTypeMap<T1,T2>(string name);
        ITypeMap<T1, T2> CreateTypeMap<T1, T2>(string name, int maxDepth);
    }
}
