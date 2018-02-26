using AutoMapper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynMapper.Benchmark
{
    public class AutoMapperBenchmark : BenchmarkBase, IBenchmark
    {
        public override string MapperName
        {
            get
            {
                return "AutoMapper";
            }
        }

        protected override BenchmarkResult SimpleTest(long count)
        {
            var result = new BenchmarkResult();
         
            var sw = new Stopwatch();
            sw.Start();            
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<RoslynMapper.Benchmark.Sample.Simple.A, RoslynMapper.Benchmark.Sample.Simple.B>();
                cfg.CreateMap<char, int>();
            });
            AutoMapper.IMapper mapper = config.CreateMapper();
            sw.Stop();

            result.Initialize = sw.ElapsedMilliseconds;

            sw.Restart();

            var s = new RoslynMapper.Benchmark.Sample.Simple.A();
            var d = new RoslynMapper.Benchmark.Sample.Simple.B();

            for (int i = 0; i < count; ++i)
            {
                d = mapper.Map<RoslynMapper.Benchmark.Sample.Simple.A, RoslynMapper.Benchmark.Sample.Simple.B>(s, d);
            }

            sw.Stop();

            result.Elapse = sw.ElapsedMilliseconds;

            return result;
        }
    }
}
