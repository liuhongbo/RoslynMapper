using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynMapper.Samples
{
    public class HelloWorld : ISample
    {
        public class A
        {
            public string Name { get; set; }
        }

        public class B
        {
            public string Name;
        }

        public string Name
        {
            get
            {
                return "Hello World";
            }
        }

        public void Run()
        {

            A a = new A() { Name = "Hello World" };

            var mapper = RoslynMapper.MapEngine.DefaultInstance;
            mapper.SetMapper<A, B>();
            //Console.WriteLine(mapper.Builder.GenerateCode());
            if (mapper.Build())
            {
                var b = mapper.Map<A, B>(a);
                Console.WriteLine(b.Name);
            }
            else
            {
                Console.WriteLine("build failed.");
            }

        }
    }
}
