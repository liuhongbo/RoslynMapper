using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoslynMapper;

namespace RoslynMapper.Map
{
    public class Mappers : Dictionary<int, IMapper>, IMappers
    {
        public IMapper GetMapper(int key)
        {
            IMapper mapper = null;
            this.TryGetValue(key, out mapper);
            return mapper;
        }

        public IMapper<T1, T2> GetMapper<T1, T2>()
        {
            return (IMapper<T1, T2>)GetMapper(new MapKey(typeof(T1), typeof(T2), null).GetHashCode());
        }

        public IMapper<T1, T2> GetMapper<T1, T2>(string name)
        {
            return (IMapper<T1, T2>)GetMapper(new MapKey(typeof(T1), typeof(T2), name).GetHashCode());
        }

        public void AddMapper(IMapper mapper)
        {
            this.Add(mapper.GetHashCode(), mapper);
        }

        public void RemoveMapper(IMapper mapper)
        {
            this.Remove(mapper.GetHashCode());
        }

        public void AddMappers(IEnumerable<IMapper> mappers)
        {
            foreach (var m in mappers)
            {
                this.AddMapper(m);
            }            
        }
    }
}
