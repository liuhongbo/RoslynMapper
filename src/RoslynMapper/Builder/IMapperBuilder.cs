using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoslynMapper.Map;

namespace RoslynMapper.Builder
{
    public interface IMapperBuilder
    {
        IEnumerable<KeyValuePair<MapKey, IMapper>> Build(IEnumerable<ITypeMap> typeMaps, IMapEngine engine);
        KeyValuePair<MapKey, IMapper> Build(ITypeMap typeMap, IMapEngine engine);

        string GenerateSourceCode(IEnumerable<ITypeMap> typeMaps);
        string GenerateSourceCode(ITypeMap typeMap);
    }
}
