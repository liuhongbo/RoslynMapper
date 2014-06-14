using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using RoslynMapper;

namespace RoslynMapper.UnitTests
{
    public class PropertyValueTypeTests: IClassFixture<MapEngineFixture>
    {
        private Destination _destination;
        private IMapEngine _mapper;
        public class Source
        {
            public int Value1 { get; set; }            
        }

        public struct Destination
        {
            public int Value1 { get; set; }            
        }

        public PropertyValueTypeTests(MapEngineFixture fixture)
        {
            _mapper = fixture.Engine;
        }
     
        [Fact]
        public void Map_Property_Value()
        {
            Guid guid = Guid.NewGuid();
            _mapper.SetMapper<Source, Destination>(guid.ToString());
            _mapper.Build();
            _destination = _mapper.GetMapper<Source, Destination>(guid.ToString()).Map(new Source { Value1 = 4});            

            Assert.Equal(_destination.Value1, 4);
        }
        
    }

    public class FieldValueTypeTests: IClassFixture<MapEngineFixture>
    {
        private Destination _destination;
        private IMapEngine _mapper;
        public class Source
        {           
            public string Value2 { get; set; }           
        }

        public struct Destination
        {
            public string Value2;
        }

        public FieldValueTypeTests(MapEngineFixture fixture)
        {
            _mapper = fixture.Engine;
        }     
       
        [Fact]
        public void Map_Field_Value()
        {
            Guid guid = Guid.NewGuid();
            _mapper.SetMapper<Source, Destination>(guid.ToString());
            _mapper.Build();
            _destination = _mapper.GetMapper<Source, Destination>(guid.ToString()).Map(new Source { Value2 = "hello world" });

            Assert.Equal(_destination.Value2, "hello world");
        }
       
    }

    public class MethodValueTypeTests: IClassFixture<MapEngineFixture>
    {
        private Destination _destination;
        private IMapEngine _mapper;
        public class Source
        {           
            public string Value3()
            {
                return "hello method";
            }
        }

        public struct Destination
        {
            public string Value3;
        }

        public MethodValueTypeTests(MapEngineFixture fixture)
        {
            _mapper = fixture.Engine;
        }     
       
        [Fact]
        public void Map_Method_Value()
        {
            Guid guid = Guid.NewGuid();
            _mapper.SetMapper<Source, Destination>(guid.ToString());
            _mapper.Build();
            _destination = _mapper.GetMapper<Source, Destination>(guid.ToString()).Map(new Source {});

            Assert.Equal(_destination.Value3, "hello method");
        }
    }
}
