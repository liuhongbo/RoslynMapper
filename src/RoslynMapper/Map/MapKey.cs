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
		string _mapName;
		int _hash;

        public MapKey(Type TypeFrom, Type TypeTo, string mapName)
		{
			_TypeFrom = TypeFrom;
			_TypeTo = TypeTo;
			_mapName = mapName;
            _hash = unchecked(((TypeFrom.GetHashCode()*397) ^ (TypeTo.GetHashCode() * 7)) + (mapName == null ? 0 : mapName.GetHashCode()));
		}

		public override bool Equals(object obj)
		{
            var rhs = (MapKey)obj;
			return _hash == rhs._hash && _TypeFrom == rhs._TypeFrom && _TypeTo == rhs._TypeTo && _mapName == rhs._mapName;
		}

		public override int GetHashCode()
		{
			return _hash;
		}
    }
}
