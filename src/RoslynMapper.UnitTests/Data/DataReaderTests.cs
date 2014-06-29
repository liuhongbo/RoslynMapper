using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Data;
using RoslynMapper.Data;
using System.Linq.Expressions;

namespace RoslynMapper.UnitTests.Data
{
    public class DataReaderTests: IClassFixture<MapEngineFixture>
    {
        private IMapEngine _mapper;
        public DataReaderTests(MapEngineFixture fixture)
        {
            _mapper = fixture.Engine;
        }

        public class Entity
        {
            public int Value1 { get; set; }
        }        

        [Fact]
        public void Test()
        {
            Guid guid = Guid.NewGuid();
            IDataReader reader = null;            
            reader.SetMapper<Entity>(_mapper,guid.ToString());
            string str = _mapper.Builder.GenerateCode();

            _mapper.Build();
            var entity = reader.Get<Entity>();
            
        }

    }
}
