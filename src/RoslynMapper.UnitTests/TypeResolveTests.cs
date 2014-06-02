using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RoslynMapper.UnitTests
{
    public class TypeResolveTests : IClassFixture<MapEngineFixture>
    {
        private IMapEngine _mapper;
        public TypeResolveTests(MapEngineFixture fixture)
        {
            _mapper = fixture.Engine;
        }

        public class Source
        {
            public int Value { get; set; }            
        }
        
        public class Destination
        {
            public int OtherValue { get; set; }
            public string OtherString { get; set; }
        }


        [Fact]
        public void Map_Type_using_Expression_Lambda_Resolver()
        {
            Guid guid = Guid.NewGuid();
            _mapper.SetMapper<Source, Destination>(guid.ToString()).Resolve((s, d) => d.OtherValue = s.Value * 4);
            _mapper.Build();

            var destination = _mapper.GetMapper<Source, Destination>(guid.ToString()).Map(new Source() { Value = 21 });
            Assert.Equal(destination.OtherValue, 84);
        }

        [Fact]
        public void Map_Type_using_Statement_Lambda_Resolver()
        {
            Guid guid = Guid.NewGuid();
            _mapper.SetMapper<Source, Destination>(guid.ToString()).Resolve((s, d) => { d.OtherValue = s.Value * 4; d.OtherString = (s.Value * 3).ToString(); });
            _mapper.Build();

            var destination = _mapper.GetMapper<Source, Destination>(guid.ToString()).Map(new Source() { Value = 21 });
            Assert.Equal(destination.OtherString, "63");
        }
    }
}
