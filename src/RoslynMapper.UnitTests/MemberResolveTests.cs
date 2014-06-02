using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RoslynMapper.UnitTests
{
    public class TopMemberResolveTests : IClassFixture<MapEngineFixture>
    {
        private IMapEngine _mapper;
        public TopMemberResolveTests(MapEngineFixture fixture)
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
        }

        [Fact]
        public void Map_Type_Member_using_Member_Resolver()
        {
            Guid guid = Guid.NewGuid();
            _mapper.SetMapper<Source, Destination>(guid.ToString()).Resolve(t2 => t2.OtherValue, (s, d) => d.OtherValue = s.Value * 5);
            _mapper.Build();

            var destination = _mapper.GetMapper<Source, Destination>(guid.ToString()).Map(new Source() { Value = 21 });
            Assert.Equal(destination.OtherValue, 105);
        }
    }
}
