using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynMapper.Benchmark
{

    public enum BenchmarkTest
    {
        Simple,
        Nested,
        Customization
    }

    public class BenchmarkResult
    {
        public BenchmarkTest Test { get; set; }
        public string MapperName { get; set; }
        public long Initialize { get; set; }
        public long Count { get; set; }
        public long Elapse { get; set; }
    }

    public interface IBenchmark
    {
        string MapperName { get; }
        BenchmarkResult Run(long count, BenchmarkTest test);
    }
}
