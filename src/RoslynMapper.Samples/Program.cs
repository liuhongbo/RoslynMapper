using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynMapper.Samples
{
    public class A
    {
        public string Name { get; set; }
    }

    public class B
    {
        public string Name { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            A a = new A();
            a.Name = "Hello world!";

            var engine = RoslynMapper.MappingEngine.DefaultInstance;

            engine.SetMapper<A, B>();
            engine.Build();

            var mapper = engine.GetMapper<A, B>();
            B b = mapper.Map(a);

            Console.WriteLine(b.Name);

            Console.ReadKey();
        }
    }
}
