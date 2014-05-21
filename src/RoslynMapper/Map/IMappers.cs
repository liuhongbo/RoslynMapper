using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynMapper.Map
{
    public interface IMappers
    {
        IMapper GetMapper(int key);
        IMapper<T1,T2> GetMapper<T1,T2>();
        IMapper<T1, T2> GetMapper<T1, T2>(string name);
        void AddMapper(IMapper mapper);
        void AddMappers(IEnumerable<IMapper> mappers);
        void RemoveMapper(IMapper mapper);        
    }
}
