using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmitMapper;
namespace RoslynMapper.Benchmark
{
    public class SimpleTest
    {
        public class A2
        {
            public string str1;
            public string str2;
            public string str3;
            public string str4;
            public string str5;
            public string str6;
            public string str7;
            public string str8;
            public string str9;

            public int n1;
            public int n2;
            public int n3;
            public int n4;
            public int n5;
            public int n6;
            public int n7;
            public int n8;
        }

        public class B2
        {
            public string str1 = "str1";
            public string str2 = "str2";
            public string str3 = "str3";
            public string str4 = "str4";
            public string str5 = "str5";
            public string str6 = "str6";
            public string str7 = "str7";
            public string str8 = "str8";
            public string str9 = "str9";

            public int n1 = 1;
            public long n2 = 2;
            public short n3 = 3;
            public byte n4 = 4;
            public decimal n5 = 5;
            public float n6 = 6;
            public int n7 = 7;
            public char n8 = 'a';
        }

        static A2 HandwrittenMap(B2 s, A2 result)
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

        static long BenchBLToolkit_Simple(int mappingsCount)
        {
            var s = new B2();
            var d = new A2();

            d = BLToolkit.Mapping.Map.ObjectToObject<A2>(s);

            var sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < mappingsCount; ++i)
            {
                d = BLToolkit.Mapping.Map.ObjectToObject<A2>(s);
            }
            sw.Stop();
            return sw.ElapsedMilliseconds;
        }

        static ObjectsMapper<B2, A2> emitMapper;

        static long EmitMapper_Simple(int mappingsCount)
        {
            var s = new B2();
            var d = new A2();

            var sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < mappingsCount; ++i)
            {
                d = emitMapper.Map(s, d);
            }
            sw.Stop();
            return sw.ElapsedMilliseconds;
        }

        static long AutoMapper_Simple(int mappingsCount)
        {
            var s = new B2();
            var d = new A2();

            var sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < mappingsCount; ++i)
            {
                d = AutoMapper.Mapper.Map<B2, A2>(s, d);
            }
            sw.Stop();
            return sw.ElapsedMilliseconds;
        }

        static long HandWtitten_Simple(int mappingsCount)
        {
            var s = new B2();
            var d = new A2();

            var sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < mappingsCount; ++i)
            {
                d = HandwrittenMap(s, d);
            }
            sw.Stop();
            return sw.ElapsedMilliseconds;
        }

        static IMapper<B2, A2> roslynMapper = null;
        static long RoslynMapper_Simple(int mappingsCount)
        {
            var s = new B2();
            var d = new A2();

            var sw = new Stopwatch();
            sw.Start();            
            for (int i = 0; i < mappingsCount; ++i)
            {
                d = roslynMapper.Map(s,d);
            }
            sw.Stop();
            return sw.ElapsedMilliseconds;
        }

        public static void Initialize()
        {
            var sw = new Stopwatch();

            sw.Start();
            AutoMapper.Mapper.CreateMap<B2, A2>();
            AutoMapper.Mapper.CreateMap<char, int>();
            sw.Start();
            Console.WriteLine("Auto Mapper (simple) init: {0} milliseconds", sw.ElapsedMilliseconds);

            sw.Restart();
            emitMapper = ObjectMapperManager.DefaultInstance.GetMapper<B2, A2>();
            sw.Stop();
            Console.WriteLine("Emit Mapper (simple) init: {0} milliseconds", sw.ElapsedMilliseconds);

            sw.Restart();
            var engine = RoslynMapper.MapEngine.DefaultInstance;
            engine.SetMapper<B2, A2>();
            engine.Build();
            roslynMapper = engine.GetMapper<B2, A2>();
            sw.Stop();
            Console.WriteLine("Roslyn Mapper (simple) init: {0} milliseconds", sw.ElapsedMilliseconds);
        }

        public static void Run()
        {
            int mappingsCount = 1000000;
            Console.WriteLine("Auto Mapper (simple): {0} milliseconds", AutoMapper_Simple(mappingsCount));
            Console.WriteLine("BLToolkit (simple): {0} milliseconds", BenchBLToolkit_Simple(mappingsCount));
            Console.WriteLine("Emit Mapper (simple): {0} milliseconds", EmitMapper_Simple(mappingsCount));
            Console.WriteLine("Handwritten Mapper (simple): {0} milliseconds", HandWtitten_Simple(mappingsCount));
            Console.WriteLine("Roslyn Mapper (simple): {0} milliseconds", RoslynMapper_Simple(mappingsCount));
        }

        /*
        Auto Mapper (simple) init: 217 milliseconds
        Emit Mapper (simple) init: 62 milliseconds
        Roslyn Mapper (simple) init: 2186 milliseconds
        Auto Mapper (simple): 37722 milliseconds
        BLToolkit (simple): 17274 milliseconds
        Emit Mapper (simple): 154 milliseconds
        Handwritten Mapper (simple): 51 milliseconds
        Roslyn Mapper (simple): 39 milliseconds
         */
    }
}
