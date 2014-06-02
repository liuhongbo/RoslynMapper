using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynMapper.Map
{
    public class MemberPath
    {
        public MemberPath(Type rootType, string accessPath)
        {
            RootType = rootType;
            AccessPath = accessPath;
        }

        public Type RootType { get; set; }
        public string AccessPath { get; set; }

        public override bool Equals(object obj)
        {
            var rhs = (MemberPath)obj;
            return (rhs.RootType.Equals(this.RootType) && (rhs.AccessPath == this.AccessPath));
        }

        public override int GetHashCode()
        {
            return (RootType.GetHashCode() * 397 + AccessPath.GetHashCode());
        }
    }
}
