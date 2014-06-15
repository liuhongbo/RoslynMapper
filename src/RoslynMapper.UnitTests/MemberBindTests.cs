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

    public class MethodMemberBindTests : IClassFixture<MapEngineFixture>
    {
        private IMapEngine _mapper;
        public MethodMemberBindTests(MapEngineFixture fixture)
        {
            _mapper = fixture.Engine;
        }

        public class Source
        {
            public int Value()
            {
                return -108;
            }
        }
        
        public class Destination
        {
            public int OtherValue { get; set; }
        }

        [Fact]
        public void Map_Method_to_Property_with_Bind()
        {
            Guid guid = Guid.NewGuid();
            _mapper.SetMapper<Source, Destination>(guid.ToString()).Bind(t1 => t1.Value(), t2 => t2.OtherValue);
            _mapper.Build();
            var destination = _mapper.GetMapper<Source, Destination>(guid.ToString()).Map(new Source());

            Assert.Equal(destination.OtherValue, -108);
        }
    }

    public class SecondLevelMethodMemberBindTests : IClassFixture<MapEngineFixture>
    {
        private IMapEngine _mapper;
        public SecondLevelMethodMemberBindTests(MapEngineFixture fixture)
        {
            _mapper = fixture.Engine;
        }

        public class A
        {
            public int InsideValue()
            {
                return 304;
            }
        }

        public class Source
        {
            public A a;
        }

        public class Destination
        {
            public int OtherValue { get; set; }
        }

        [Fact]
        public void Map_with_Secong_Layer_Method_to_Property_with_Bind()
        {
            Guid guid = Guid.NewGuid();
            _mapper.SetMapper<Source, Destination>(guid.ToString()).Bind(t1 => t1.a.InsideValue(), t2 => t2.OtherValue);
            _mapper.Build();
            var destination = _mapper.GetMapper<Source, Destination>(guid.ToString()).Map(new Source() { a = new A() });

            Assert.Equal(destination.OtherValue, 304);
        }
    }
}
