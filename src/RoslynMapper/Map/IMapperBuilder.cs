using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynMapper.Map
{
    public interface IMapperBuilder
    {
        IEnumerable<KeyValuePair<MapKey, IMapper>> Build(IEnumerable<ITypeMap> typeMaps);
        KeyValuePair<MapKey, IMapper> Build(ITypeMap typeMap);
    }
}
