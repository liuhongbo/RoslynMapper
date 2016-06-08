using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoslynMapper.Map;
using RoslynMapper.Builder;

namespace RoslynMapper
{
    public class MapEngine: IMapEngine, IBuilder
    {
        private static IMapEngine _defaultInstance = null;
        private ITypeMapFactory _typeMapFactory = null;
        private IMappers _mappers = null;
        private ITypeMaps _typeMaps = null;
        private IMapperBuilder _mapperBuidler = null;

        public MapEngine()
        {
            _typeMapFactory = new TypeMapFactory();
            _mappers = new Mappers();
            _typeMaps = new TypeMaps();
            _mapperBuidler = new MapperBuilder();
        }

        public static IMapEngine DefaultInstance
        {
            get
            {
                if (_defaultInstance == null)
                {
                    lock (typeof(MapEngine))
                    {
                        if (_defaultInstance == null)
                        {
                            _defaultInstance = new MapEngine();
                        }
                    }
                }
                return _defaultInstance;
            }
        }

        #region IMapEngine Interface

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
            ITypeMap<T1,T2> typeMap = null;

            typeMap = _typeMaps.GetTypeMap<T1,T2>(name);

            if (typeMap == null)
            {
                typeMap = _typeMapFactory.CreateTypeMap<T1, T2>(name);
                _typeMaps.AddTypeMap(typeMap);
            }

            return new Mapping<T1, T2>(typeMap);
        }

        public bool Build()
        {
            var mappers = _mapperBuidler.Build(_typeMaps.GetTypeMaps().Where(m=>(_mappers.GetMapper(m.Key)==null)), this);
            _mappers.AddMappers(mappers);
            return (mappers.Count()>0);
        }

        public T2 Map<T1, T2>(T1 t1)
        {
            var mapper = GetMapper<T1, T2>();
            if (mapper == null) return default(T2);
            return mapper.Map(t1);
        }

        public T2 Map<T1, T2>(T1 t1, T2 t2)
        {
            var mapper = GetMapper<T1, T2>();
            if (mapper == null) return default(T2);
            return mapper.Map(t1, t2);
        }

        public T2 Map<T1, T2>(string name, T1 t1)
        {
            var mapper = GetMapper<T1, T2>(name);
            if (mapper == null) return default(T2);
            return mapper.Map(t1);
        }

        public T2 Map<T1, T2>(string name, T1 t1, T2 t2)
        {
            var mapper = GetMapper<T1, T2>(name);
            if (mapper == null) return default(T2);
            return mapper.Map(t1, t2);
        }

        #endregion

        #region IBuilder Inteface

        public IBuilder Builder
        {
            get
            {
                return (IBuilder)this;
            }
        }

        public string GenerateCode()
        {
            return _mapperBuidler.GenerateSourceCode(_typeMaps.GetTypeMaps());
        }

        public bool GenerateAssembly(string path)
        {

            return true;
        }

        #endregion
    }
}
