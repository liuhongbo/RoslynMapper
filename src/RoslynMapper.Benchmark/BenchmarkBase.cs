using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynMapper.Benchmark
{
    public abstract class BenchmarkBase : IBenchmark
    {
        public abstract string MapperName
        {
            get;

        }

        public virtual BenchmarkResult Run(long count, BenchmarkTest test)
        {
            BenchmarkResult result = null;
            switch (test)
            {
                case BenchmarkTest.Simple:
                    result = SimpleTest(count);
                    break;
            }

            if (result != null)
            {
                result.MapperName = MapperName;
                result.Test = test;
                result.Count = count;
            }

            return result;
        }

        protected virtual BenchmarkResult SimpleTest(long count)
        {
            return null;
        }
    }
}
