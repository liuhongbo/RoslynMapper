using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynMapper
{
    public interface IBuilder
    {
        string GenerateCode();
        bool GenerateAssembly(string path);
    }
}
