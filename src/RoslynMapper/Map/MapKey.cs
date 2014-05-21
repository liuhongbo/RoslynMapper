using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynMapper.Map
{
    public class MapKey
    {
        Type _TypeFrom;
		Type _TypeTo;
		string _mapperName;
		int _hash;

        public MapKey(Type TypeFrom, Type TypeTo, string mapperName)
		{
			_TypeFrom = TypeFrom;
			_TypeTo = TypeTo;
			_mapperName = mapperName;
			_hash = TypeFrom.GetHashCode() + TypeTo.GetHashCode() + (mapperName == null ? 0 : mapperName.GetHashCode());
		}

		public override bool Equals(object obj)
		{
            var rhs = (MapKey)obj;
			return _hash == rhs._hash && _TypeFrom == rhs._TypeFrom && _TypeTo == rhs._TypeTo && _mapperName == rhs._mapperName;
		}

		public override int GetHashCode()
		{
			return _hash;
		}
    }
}
