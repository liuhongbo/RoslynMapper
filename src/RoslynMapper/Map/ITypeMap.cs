using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynMapper.Map
{
    public interface ITypeMap
    {
        string Name { get; set; }
        MapKey Key { get; }
        Type SourceType { get; }
        Type DestinationType { get; }
        IMembers Members { get; }
        string CreateMapper();
        string MapperName { get; }
    }

    public interface ITypeMap<T1, T2> : ITypeMap
    {
        Action<T1, T2> Resolver { get; set; }
    }
}
