using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynMapper.Benchmark.Sample
{
    public class Simple
    {
        public class A
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

        public class B
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
    }
}
