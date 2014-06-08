using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynMapper.Benchmark
{
    public static class TestHelper
    {
        public static IEnumerable<BenchmarkResult> Run(BenchmarkTest test, long count)
        {
            var results = new List<BenchmarkResult>();

            var benchmarks = typeof(IBenchmark).Assembly.ExportedTypes.Where(t => (!t.IsAbstract) && (typeof(IBenchmark).IsAssignableFrom(t)) && (!t.IsInterface));

            foreach (var benchmark in benchmarks)
            {
                var result = (Activator.CreateInstance(benchmark) as IBenchmark).Run(count, test);
                if (result != null)
                {
                    results.Add(result);
                }
            }

            return results;
        }

        public static IEnumerable<BenchmarkResult> Run(long count)
        {
            var results = new List<BenchmarkResult>();

            foreach (var t in (BenchmarkTest[])Enum.GetValues(typeof(BenchmarkTest)))
            {
                results.AddRange(Run(t, count));
            }

            return results;
        }
    }
}
