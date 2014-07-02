using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RoslynMapper.UnitTests
{    
    public class GuidMapTests : IClassFixture<MapEngineFixture>
    {
        private IMapEngine _mapper;

        public GuidMapTests(MapEngineFixture fixture)
        {
            _mapper = fixture.Engine;
        }

        public class Source
        {
            public Guid Value1 { get; set; }
        }

        public struct Destination
        {
            public Guid Value2 { get; set; }
        }

        [Fact]
        public void Map_Guid_Value()
        {
            Guid guid = Guid.NewGuid();
            _mapper.SetMapper<Source, Destination>(guid.ToString());
            _mapper.Build();

            Guid value = new Guid();

            var destination = _mapper.GetMapper<Source, Destination>(guid.ToString()).Map(new Source { Value1 = value });

            Assert.Equal(destination.Value2, value);
        }
    }
}
