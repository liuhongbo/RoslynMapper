using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynMapper.Map
{
    public interface ITypeMaps
    {
        ITypeMap GetTypeMap(MapKey key);
        ITypeMap<T1, T2> GetTypeMap<T1, T2>();
        ITypeMap<T1, T2> GetTypeMap<T1, T2>(string name);
        void AddTypeMap(ITypeMap typeMap);
        void RemoveTypeMap(ITypeMap typeMap);
        IEnumerable<ITypeMap> GetTypeMaps();
    }
}
