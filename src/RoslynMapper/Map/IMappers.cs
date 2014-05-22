using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynMapper.Map
{
    public interface IMappers
    {
        IMapper GetMapper(MapKey key);
        IMapper<T1,T2> GetMapper<T1,T2>();
        IMapper<T1, T2> GetMapper<T1, T2>(string name);
        void AddMapper(MapKey key, IMapper mapper);
        void AddMappers(IEnumerable<KeyValuePair<MapKey, IMapper>> mappers);
        void RemoveMapper(MapKey key);        
    }
}
