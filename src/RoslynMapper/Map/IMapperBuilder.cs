using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynMapper.Map
{
    public interface IMapperBuilder
    {
        IEnumerable<IMapper> Build(IEnumerable<ITypeMap> typeMaps);
        IMapper Build(ITypeMap typeMap);
    }
}
