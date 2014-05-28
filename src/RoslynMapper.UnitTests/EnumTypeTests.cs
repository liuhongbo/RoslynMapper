using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RoslynMapper.UnitTests
{
    public class EnumTypeMemberConvert:IClassFixture<MapEngineFixture>
    {        
        private IMapEngine _mapper;

        public enum Days { Sat, Sun, Mon, Tue, Wed, Thu, Fri };

        public class Source
        {
            public Days value { get; set; }           
        }

        public struct Destination
        {
            public int value { get; set; }
        }

        public EnumTypeMemberConvert(MapEngineFixture fixture)
        {
            _mapper = fixture.Engine;
        }

        [Fact]
        public void Map_TypeMember_From_Enum_to_Int()
        {
            Guid guid = Guid.NewGuid();
            _mapper.SetMapper<Source, Destination>(guid.ToString());
            _mapper.Build();
            var destination = _mapper.GetMapper<Source, Destination>(guid.ToString()).Map(new Source { value =  Days.Thu });

            Assert.Equal(destination.value, 5);
        }

        [Fact]
        public void Map_TypeMember_From_Int_to_Enum()
        {
            Guid guid = Guid.NewGuid();
            _mapper.SetMapper<Destination, Source>(guid.ToString());
            _mapper.Build();
            var source = _mapper.GetMapper<Destination, Source>(guid.ToString()).Map(new Destination { value = 4 });

            Assert.Equal(source.value, Days.Wed);
        }

        public struct Destination2
        {
            public byte value { get; set; }
        }

        [Fact]
        public void Map_TypeMember_From_Enum_to_Byte()
        {
            Guid guid = Guid.NewGuid();
            _mapper.SetMapper<Source, Destination>(guid.ToString());
            _mapper.Build();
            var destination = _mapper.GetMapper<Source, Destination>(guid.ToString()).Map(new Source { value = Days.Thu });

            Assert.Equal(destination.value, 5);
        }

        [Fact]
        public void Map_TypeMember_From_Byte_to_Enum()
        {
            Guid guid = Guid.NewGuid();
            _mapper.SetMapper<Destination, Source>(guid.ToString());
            _mapper.Build();
            var source = _mapper.GetMapper<Destination, Source>(guid.ToString()).Map(new Destination { value = 4 });

            Assert.Equal(source.value, Days.Wed);
        }
    }
}
