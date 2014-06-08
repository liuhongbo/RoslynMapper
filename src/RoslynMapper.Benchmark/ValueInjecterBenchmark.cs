using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;

namespace RoslynMapper.Benchmark
{
    public class ValueInjecterBenchmark :BenchmarkBase, IBenchmark
    {
        public override string MapperName
        {
            get
            {
                return "ValueInjecter";
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

            for (int i = 0; i < count; ++i)
            {
                d = (RoslynMapper.Benchmark.Sample.Simple.B)d.InjectFrom(s);
            }

            sw.Stop();

            result.Elapse = sw.ElapsedMilliseconds;

            return result;
        }
    }
}
