using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using RoslynMapper;

namespace RoslynMapper.UnitTests
{
    public class MappingEngineTests : IClassFixture<MappingEngineFixture>
    {
        private IMapEngine _mapper;

        public MappingEngineTests(MappingEngineFixture fixture)
        {
            _mapper = fixture.Engine;
        }

        public class Source
        {            
            public string Value { get; set; }
        }

        public struct Destination
        {            
            public string Value;
        }

        [Fact]
        public void SetMapper()
        {
            Guid guid = Guid.NewGuid();
            _mapper.SetMapper<Source, Destination>(guid.ToString());
            _mapper.Build();
            
            string value = Guid.NewGuid().ToString();
            var _destination = _mapper.GetMapper<Source, Destination>(guid.ToString()).Map(new Source() { Value = value });
            Assert.Equal(_destination.Value, value);
        }
    }
}
