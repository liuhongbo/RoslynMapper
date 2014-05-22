using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoslynMapper;

namespace RoslynMapper.Map
{
    public class Mappers : Dictionary<MapKey, IMapper>, IMappers
    {
        public IMapper GetMapper(MapKey key)
        {
            IMapper mapper = null;
            this.TryGetValue(key, out mapper);
            return mapper;
        }

        public IMapper<T1, T2> GetMapper<T1, T2>()
        {
            return (IMapper<T1, T2>)GetMapper(new MapKey(typeof(T1), typeof(T2), null));
        }

        public IMapper<T1, T2> GetMapper<T1, T2>(string name)
        {
            return (IMapper<T1, T2>)GetMapper(new MapKey(typeof(T1), typeof(T2), name));
        }

        public void AddMapper(MapKey key, IMapper mapper)
        {
            this.Add(key, mapper);
        }

        public void RemoveMapper(MapKey key)
        {
            this.Remove(key);
        }

        public void AddMappers(IEnumerable<KeyValuePair<MapKey, IMapper>> mappers)
        {
            foreach (var m in mappers)
            {
                this.AddMapper(m.Key, m.Value);
            }            
        }
    }
}
