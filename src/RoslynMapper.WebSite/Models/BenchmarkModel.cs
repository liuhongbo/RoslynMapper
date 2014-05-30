using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RoslynMapper.WebSite.Models
{
    public class BenchmarkResult
    {
        public string Name { get; set; }
        public long Initialize { get; set; }
        public long Count { get; set; }
        public long Elapse { get; set; }
    }

    public class BenchmarkModel
    {
        public BenchmarkModel()
        {
            Results = new List<BenchmarkResult>();
        }
        public IList<BenchmarkResult> Results { get; set; }        
    }
}