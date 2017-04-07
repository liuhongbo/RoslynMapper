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

        //MemberInfo got from lambda expression has a different RefectedType
        //http://stackoverflow.com/questions/23105567/reflectedtype-from-memberexpression
        public MemberKey(MemberInfo memberInfo, MemberPath path)
		{
			_memberInfo = memberInfo;
            _path = path;
            //_hash = memberInfo.GetHashCode() * 397 + _path.GetHashCode();
            _hash = unchecked(((memberInfo.Module.GetHashCode() * 397) ^ (memberInfo.MetadataToken * 7)) + _path.GetHashCode());
        }

		public override bool Equals(object obj)
		{
            var rhs = (MemberKey)obj;
            //return _hash == rhs._hash && rhs._memberInfo == this._memberInfo && rhs._path.Equals(this._path);
            return _hash == rhs._hash && rhs._memberInfo.MetadataToken == this._memberInfo.MetadataToken && rhs._path.Equals(this._path);
		}

		public override int GetHashCode()
		{
			return _hash;
		}
    }
}
