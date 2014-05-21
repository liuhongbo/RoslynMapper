using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynMapper.Map
{
    public class TypeMaps : Dictionary<int, ITypeMap>, ITypeMaps
    {
        public ITypeMap GetTypeMap(int key)
        {
            ITypeMap typeMap = null;
            this.TryGetValue(key, out typeMap);
            return typeMap;
        }

        public ITypeMap<T1, T2> GetTypeMap<T1, T2>()
        {
            return (ITypeMap<T1, T2>)GetTypeMap(new MapKey(typeof(T1), typeof(T2), null).GetHashCode());
        }

        public ITypeMap<T1, T2> GetTypeMap<T1, T2>(string name)
        {
            return (ITypeMap<T1, T2>)GetTypeMap(new MapKey(typeof(T1), typeof(T2), name).GetHashCode());
        }

        public void AddTypeMap(ITypeMap typeMap)
        {
            this.Add(typeMap.Key, typeMap);
        }

        public void RemoveTypeMap(ITypeMap typeMap)
        {
            this.Remove(typeMap.Key);
        }

        public IEnumerable<ITypeMap> GetTypeMaps()
        {
            var typeMaps = new List<ITypeMap>(this.Count);
            foreach (var typeMap in this)
            {
                typeMaps.Add(typeMap.Value);
            }
            return typeMaps;
        }
    }
}
