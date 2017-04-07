using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynMapper.Map
{
    public class MapKey
    {
        Type _typeFrom;
		Type _typeTo;
		string _mapName;
		int _hash;

        public MapKey(Type typeFrom, Type typeTo, string mapName)
		{
			_typeFrom = typeFrom;
			_typeTo = typeTo;
			_mapName = mapName;
            _hash = unchecked(((typeFrom.GetHashCode()*397) ^ (typeTo.GetHashCode() * 7)) + (mapName == null ? 0 : mapName.GetHashCode()));
		}

		public override bool Equals(object obj)
		{
            var rhs = (MapKey)obj;
			return _hash == rhs._hash && _typeFrom == rhs._typeFrom && _typeTo == rhs._typeTo && _mapName == rhs._mapName;
		}

		public override int GetHashCode()
		{
			return _hash;
		}
    }
}
