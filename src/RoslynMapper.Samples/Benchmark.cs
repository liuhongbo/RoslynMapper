using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoslynMapper.Benchmark;

namespace RoslynMapper.Samples
{
    public class Benchmark : ISample
    {
        public string Name
        {
            get
            {
                return "Benchmark";
            }
        }

        public void Run()
        {
            long count = 1000000;

            var results = TestHelper.Run(count);

            Console.WriteLine("Benchmark test for {0} times mapping", count);

            foreach (var result in results)
            {
                Console.WriteLine("{0,16} {1,12} {2,8} {3,8}", result.MapperName, result.Test.ToString(), result.Initialize, result.Elapse);
            }
        }
    }
}
