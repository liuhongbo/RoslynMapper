using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoslynMapper;

namespace RoslynMapper.UnitTests
{
    public class MappingEngineFixture :IDisposable
    {
        public IMapEngine Engine
        {
            get
            {
                return RoslynMapper.MapEngine.DefaultInstance;
            }
        }

        public void Dispose()
        {

        }
    }
}
