using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynMapper.Benchmark
{
    public class BLToolkitBenchmark : BenchmarkBase, IBenchmark
    {
        public override string MapperName
        {
            get
            {
                return "BLToolkit";
            }
        }

        protected override BenchmarkResult SimpleTest(long count)
        {
            var result = new BenchmarkResult();

            var sw = new Stopwatch();
            sw.Start();
            
            sw.Stop();

            result.Initialize = sw.ElapsedMilliseconds;

            sw.Restart();

            var s = new RoslynMapper.Benchmark.Sample.Simple.A();
            var d = new RoslynMapper.Benchmark.Sample.Simple.B();

            var map = BLToolkit.Mapping.Map.GetObjectMapper<RoslynMapper.Benchmark.Sample.Simple.A, RoslynMapper.Benchmark.Sample.Simple.B>();

            for (int i = 0; i < count; ++i)
            {
                d = map(s);
            }

            sw.Stop();

            result.Elapse = sw.ElapsedMilliseconds;

            return result;
        }
    }
}
