using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RoslynMapper.Benchmark;

namespace RoslynMapper.WebSite.Controllers
{

    public class BenchmarkController : Controller
    {
       
        public ActionResult Index(BenchmarkTest test = BenchmarkTest.Simple)
        {
            return View();
        }


        public ActionResult Test(BenchmarkTest test)
        {
            long count = 1000000;
            var results = RoslynMapper.Benchmark.TestHelper.Run(test, count);

            return View(results);
        }

    }
}