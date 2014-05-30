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
        public int Age { get; set; }
        public string test;
    }

    public class B
    {
        public string Name { get; set; }

        public decimal Age;

        public string test;
    }

    class Program
    {
        static void Main(string[] args)
        {
            A a = new A();
            a.Name = "Hello world!";
            a.Age = 45;
            a.test = "test";

            var engine = RoslynMapper.MapEngine.DefaultInstance;

            engine.SetMapper<A, B>().Ignore(m => ((B)m).test);
            //engine.SetMapper<B, A>();

            Console.WriteLine(engine.Builder.GenerateCode());

            engine.Build();

            var mapper = engine.GetMapper<A, B>();
            B b = mapper.Map(a);

            Console.WriteLine(b.Name);

            Console.ReadKey();
        }
    }
}
