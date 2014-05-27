using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using RoslynMapper;

namespace RoslynMapper.UnitTests
{
    public class ValueTypesTests: IClassFixture<MapEngineFixture>
    {
        private Destination _destination;
        private IMapEngine _mapper;
        public class Source
        {
            public int Value1 { get; set; }
            public string Value2 { get; set; }
        }

        public struct Destination
        {
            public int Value1 { get; set; }
            public string Value2;
        }

        public ValueTypesTests(MapEngineFixture fixture)
        {
            _mapper = fixture.Engine;
        }
     
        [Fact]
        public void PropertyValue()
        {
            Guid guid = Guid.NewGuid();
            _mapper.SetMapper<Source, Destination>(guid.ToString());
            _mapper.Build();
            _destination = _mapper.GetMapper<Source, Destination>(guid.ToString()).Map(new Source { Value1 = 4, Value2 = "hello" });            

            Assert.Equal(_destination.Value1, 4);
        }

        [Fact]
        public void FieldValue()
        {
            Guid guid = Guid.NewGuid();
            _mapper.SetMapper<Source, Destination>(guid.ToString());
            _mapper.Build();
            _destination = _mapper.GetMapper<Source, Destination>(guid.ToString()).Map(new Source { Value1 = 4, Value2 = "hello world" });

            Assert.Equal(_destination.Value2, "hello world");
        }
    }
}
