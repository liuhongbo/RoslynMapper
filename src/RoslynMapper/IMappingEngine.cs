using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynMapper
{
    public interface IMappingEngine
    {
        //IMappingConfiguration configuration { get; set; }

        IMapper<T1, T2> GetMapper<T1, T2>();
        IMapper<T1, T2> GetMapper<T1, T2>(string name);

        IMapping<T1, T2> SetMapper<T1, T2>();
        IMapping<T1, T2> SetMapper<T1, T2>(string name);

        bool Build();
    }
}
