using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynMapper.Benchmark
{
    public class EmitMapperBenchmark : BenchmarkBase, IBenchmark
    {
        public override string MapperName
        {
            get
            {
                return "EmitMapper";
            }
        }

        protected override BenchmarkResult SimpleTest(long count)
        {
            var result = new BenchmarkResult();

            var sw = new Stopwatch();
            sw.Start();
            EmitMapper.ObjectsMapper<RoslynMapper.Benchmark.Sample.Simple.A, RoslynMapper.Benchmark.Sample.Simple.B> emitMapper;            
            sw.Stop();

            result.Initialize = sw.ElapsedMilliseconds;

            sw.Restart();

            var s = new RoslynMapper.Benchmark.Sample.Simple.A();
            var d = new RoslynMapper.Benchmark.Sample.Simple.B();
            emitMapper = EmitMapper.ObjectMapperManager.DefaultInstance.GetMapper<RoslynMapper.Benchmark.Sample.Simple.A, RoslynMapper.Benchmark.Sample.Simple.B>();

            for (int i = 0; i < count; ++i)
            {               
                d = emitMapper.Map(s, d);
            }

            sw.Stop();

            result.Elapse = sw.ElapsedMilliseconds;

            return result;
        }
    }
}
