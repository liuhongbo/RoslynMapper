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
        int _hash;

        public MemberKey(MemberInfo memberInfo)
		{
			_memberInfo = memberInfo;
            _hash = memberInfo.GetHashCode();
		}

		public override bool Equals(object obj)
		{
            var rhs = (MemberKey)obj;
            return _hash == rhs._hash && rhs._memberInfo == this._memberInfo;
		}

		public override int GetHashCode()
		{
			return _hash;
		}
    }
}
