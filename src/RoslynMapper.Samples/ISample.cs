using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynMapper.Samples
{
    public interface ISample
    {
        string Name { get; }
        void Run();
    }
}
