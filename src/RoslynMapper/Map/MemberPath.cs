using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynMapper.Map
{
    public class MemberPath
    {

        public MemberPath(Type originType, string accessPath)
        {
            OriginType = originType;
            AccessPath = accessPath;
        }

        public Type OriginType { get; set; }
        public string AccessPath { get; set; }

        public override bool Equals(object obj)
        {
            var mp = (MemberPath)obj;
            return (mp.OriginType.Equals(this.OriginType) && (mp.AccessPath == this.AccessPath));
        }

        public override int GetHashCode()
        {
            return (OriginType.GetHashCode() * 397 + AccessPath.GetHashCode());
        }
    }
}
