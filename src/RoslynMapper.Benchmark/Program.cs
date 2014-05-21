using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynMapper.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            SimpleTest.Initialize();
            //NestedClassesTest.Initialize();
            //CustomizationTest.Initialize();

            SimpleTest.Run();
            //NestedClassesTest.Run();
            //CustomizationTest.Run();
            
            Console.ReadLine();
        }
    }
}
