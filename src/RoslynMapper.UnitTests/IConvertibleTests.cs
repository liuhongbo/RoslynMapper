using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RoslynMapper.UnitTests
{
    public class StringMemberToInt : IClassFixture<MapEngineFixture>
    {
        private IMapEngine _mapper = null;
        public StringMemberToInt(MapEngineFixture fixture)
        {
            _mapper = fixture.Engine;
        }

        public class Source
        {
            public string value { get; set; }
        }

        public class Destination
        {
            public int value { get; set; }
        }

        [Fact]
        public void Map_TypeMember_From_String_to_Int_Using_IConvertible()
        {
            Guid guid = Guid.NewGuid();
            _mapper.SetMapper<Source, Destination>(guid.ToString());
            _mapper.Build();
            var destination = _mapper.GetMapper<Source, Destination>(guid.ToString()).Map(new Source { value = "20" });

            Assert.Equal(destination.value, 20);
        }
   
    }
}
