using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RoslynMapper.UnitTests
{
    public class MemberBindTests: IClassFixture<MapEngineFixture>
    {
        private IMapEngine _mapper;
        public MemberBindTests(MapEngineFixture fixture)
        {
            _mapper = fixture.Engine;
        }

        public class Source
        {
            public int Value { get; set; }
            public A a { get; set; }
        }

        public class A
        {
            public int InsideValue { get; set; }
        }

        public class Destination
        {
            public int OtherValue { get; set; }
        }

        [Fact]
        public void Map_Failed_When_Bind_Not_Specified()
        {
            Guid guid = Guid.NewGuid();
            _mapper.SetMapper<Source, Destination>(guid.ToString());
            _mapper.Build();
            var destination = _mapper.GetMapper<Source, Destination>(guid.ToString()).Map(new Source() { Value = 102 });

            Assert.NotEqual(destination.OtherValue, 102);
        }

        [Fact]
        public void Map_with_Top_Level_Member_Bind()
        {
            Guid guid = Guid.NewGuid();
            _mapper.SetMapper<Source, Destination>(guid.ToString()).Bind(t1=>t1.Value, t2=>t2.OtherValue);
            _mapper.Build();
            var destination = _mapper.GetMapper<Source, Destination>(guid.ToString()).Map(new Source() { Value = 102 });

            Assert.Equal(destination.OtherValue, 102);
        }

        [Fact]
        public void Map_with_Second_Level_Member_Bind()
        {
            Guid guid = Guid.NewGuid();
            _mapper.SetMapper<Source, Destination>(guid.ToString()).Bind(t1 => t1.a.InsideValue, t2 => t2.OtherValue);
            _mapper.Build();
            var destination = _mapper.GetMapper<Source, Destination>(guid.ToString()).Map(new Source() { Value = 102, a = new A() { InsideValue = 309 } });

            Assert.Equal(destination.OtherValue, 309);
        }
    }
}
