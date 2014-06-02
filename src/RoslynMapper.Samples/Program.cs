using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using RoslynMapper.Map;

namespace RoslynMapper.Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            var samples =  Assembly.GetExecutingAssembly().GetTypes().Where(t =>( typeof(ISample).IsAssignableFrom(t) && (t != typeof(ISample)))).ToArray();
            
            while (true)
            {
                Console.WriteLine("Please select an example to run \r\n--------------------------------------------");
                for (int i = 0; i < samples.Length; i++)
                {
                    ISample sample =(ISample) Activator.CreateInstance(samples[i]);
                    Console.WriteLine("{0}) {1}", i + 1, sample.Name);
                }
                Console.WriteLine("0) Exit\r\n--------------------------------------------\r\n");

                var key = Console.ReadKey(true);
                int index = 0;
                if (int.TryParse(key.KeyChar.ToString(), out index))
                {
                    if (index == 0) break;
                    if ((index <= samples.Length) && (index>0))
                    {
                        ISample sampleToRun = (ISample)Activator.CreateInstance(samples[index-1]);
                        sampleToRun.Run();
                    }
                }
               
            }            
        }
    }
}
