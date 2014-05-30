using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RoslynMapper.WebSite.Models;

namespace RoslynMapper.WebSite.Controllers
{

    public class BenchmarkController : Controller
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
                d = roslynMapper.Map(s, d);
            }
            sw.Stop();
            return sw.ElapsedMilliseconds;
        }

        public ActionResult Index()
        {
            return RedirectToAction("Simple");
        }

        public ActionResult Simple()
        {
            BenchmarkModel model = new BenchmarkModel();
            var sw = new Stopwatch();

            sw.Start();
            var engine = RoslynMapper.MapEngine.DefaultInstance;
            engine.SetMapper<B2, A2>();
            engine.Build();
            roslynMapper = engine.GetMapper<B2, A2>();
            sw.Stop();

            BenchmarkResult result = new BenchmarkResult();
            result.Name = "RoslynMapper";
            result.Initialize = sw.ElapsedMilliseconds;

            int mappingsCount = 1000000;
            result.Count = mappingsCount;
            result.Elapse = RoslynMapper_Simple(mappingsCount);

            model.Results.Add(result);

            return View(model);
        }
    }
}