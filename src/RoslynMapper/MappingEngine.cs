using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoslynMapper.Map;

namespace RoslynMapper
{
    public class MappingEngine:IMappingEngine
    {
        private static IMappingEngine _defaultInstance = null;
        private ITypeMapFactory _typeMapFactory = null;
        private IMappers _mappers = null;
        private ITypeMaps _typeMaps = null;
        private IMapperBuilder _mapperBuidler = null;

        public MappingEngine()
        {
            _typeMapFactory = new TypeMapFactory();
            _mappers = new Mappers();
            _typeMaps = new TypeMaps();
            _mapperBuidler = new MapperBuilder();
        }

        public static IMappingEngine DefaultInstance
        {
            get
            {
                if (_defaultInstance == null)
                {
                    lock (typeof(MappingEngine))
                    {
                        if (_defaultInstance == null)
                        {
                            _defaultInstance = new MappingEngine();
                        }
                    }
                }
                return _defaultInstance;
            }
        }

        #region IMappingEngine Interface

        public IMapper<T1, T2> GetMapper<T1, T2>()
        {
            return GetMapper<T1, T2>(null);
        }

        public IMapper<T1, T2> GetMapper<T1, T2>(string name)
        {
            return _mappers.GetMapper<T1, T2>(name);
        }

        public IMapping<T1,T2> SetMapper<T1, T2>()
        {
            return SetMapper<T1, T2>(null);
        }

        public IMapping<T1,T2> SetMapper<T1, T2>(string name)
        {
            var typeMap = _typeMapFactory.CreateTypeMap<T1, T2>(name);
            _typeMaps.AddTypeMap(typeMap);

            return new Mapping<T1, T2>(typeMap);
        }

        public bool Build()
        {
            var mappers = _mapperBuidler.Build(_typeMaps.GetTypeMaps());
            _mappers.AddMappers(mappers);
            return true;
        }

        #endregion


    }
}
