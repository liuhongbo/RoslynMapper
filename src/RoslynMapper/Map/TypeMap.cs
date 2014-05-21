using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynMapper.Map
{
    public class TypeMap<T1,T2> : ITypeMap<T1,T2>
    {
        public TypeMap(): this(null)
        {

        }

        public TypeMap(string name)
        {            
            Name = name;
        }
        public string Name { get; set; }

        public int Key
        {
            get
            {
                return new MapKey(typeof(T1), typeof(T2), Name).GetHashCode();
            }
        }

        public Type SourceType
        {
            get
            {
                return typeof(T1);
            }
        }

        public Type DestinationType
        {
            get
            {
                return typeof(T2);
            }
        }

        public override int GetHashCode()
        {
            return Key;
        }
    
    }
}
