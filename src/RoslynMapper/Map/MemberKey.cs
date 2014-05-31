using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RoslynMapper.Map
{
    public class MemberKey
    {
        MemberInfo _memberInfo;
        MemberPath _path;
        int _hash;

        public MemberKey(MemberInfo memberInfo, MemberPath path)
		{
			_memberInfo = memberInfo;
            _path = path;
            _hash = memberInfo.GetHashCode() * 397 + _path.GetHashCode();            
		}

		public override bool Equals(object obj)
		{
            var rhs = (MemberKey)obj;
            return _hash == rhs._hash && rhs._memberInfo == this._memberInfo && rhs._path.Equals(this._path);
		}

		public override int GetHashCode()
		{
			return _hash;
		}
    }
}
