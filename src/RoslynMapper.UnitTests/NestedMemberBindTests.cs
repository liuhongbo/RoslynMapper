using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RoslynMapper.UnitTests
{
    public class NestedMemberBindTests : IClassFixture<MapEngineFixture>
    {
        private IMapEngine _mapper;
        public NestedMemberBindTests(MapEngineFixture fixture)
        {
            _mapper = fixture.Engine;
        }

        public class Source
        {
            public class Int1
            {
                public int i = 10;
            }

            public class Int2
            {
                public int i = 20;
            }

            public Int1 i1 = new Int1();
            public Int2 i2 = new Int2();
        }

        public class Destination
        {
            public class Int1
            {
                public int i;
            }

            public Int1 i1 { get; set; }
        }

        [Fact]
        public void Map_Nested_Member_with_Custom_Bind()
        {
            Guid guid = Guid.NewGuid();
            _mapper.SetMapper<Source, Destination>(guid.ToString()).Bind(t1 => t1.i2, t2 => t2.i1);
            _mapper.Build();

            var destination = _mapper.GetMapper<Source, Destination>(guid.ToString()).Map(new Source() { });
            Assert.Equal(destination.i1.i, 20);
        }
    }
}
