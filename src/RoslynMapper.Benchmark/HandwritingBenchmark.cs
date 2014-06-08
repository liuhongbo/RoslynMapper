using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynMapper.Benchmark
{
    public class HandwritingBenchmark : BenchmarkBase, IBenchmark
    {
        public override string MapperName
        {
            get
            {
                return "Handwriting";
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
                d = HandwrittenMap(s, d);
            }

            sw.Stop();

            result.Elapse = sw.ElapsedMilliseconds;

            return result;
        }

        private RoslynMapper.Benchmark.Sample.Simple.B HandwrittenMap(RoslynMapper.Benchmark.Sample.Simple.A s, RoslynMapper.Benchmark.Sample.Simple.B result)
        {
            result.str1 = s.str1;
            result.str2 = s.str2;
            result.str3 = s.str3;
            result.str4 = s.str4;
            result.str5 = s.str5;
            result.str6 = s.str6;
            result.str7 = s.str7;
            result.str8 = s.str8;
            result.str9 = s.str9;

            result.n1 = s.n1;
            result.n2 = (int)s.n2;
            result.n3 = s.n3;
            result.n4 = s.n4;
            result.n5 = (int)s.n5;
            result.n6 = (int)s.n6;
            result.n7 = s.n7;
            result.n8 = s.n8;

            return result;
        }
    }
}
