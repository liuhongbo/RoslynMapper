using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RoslynMapper.UnitTests
{
    public class MemberIgnoreTests : IClassFixture<MapEngineFixture>
    {
        private IMapEngine _mapper;
        public MemberIgnoreTests(MapEngineFixture fixture)
        {
            _mapper = fixture.Engine;
        }

        public class Source : B
        {
            public int Value { get; set; }
            public A a { get; set; }
            
        }

        public class A
        {
            public int OtherValue { get; set; }
        }

        public class B
        {            
            public C c { get; set; }
        }

        public class C
        {
            public int OtherValue2 { get; set; }
        }

        public class Destination
        {
            public int Value { get; set; }
            public int OtherValue { get; set; }
            public int OtherValue2 { get; set; }
        }

        [Fact]
        public void Map_Without_Ignore()
        {
            Guid guid = Guid.NewGuid();
            _mapper.SetMapper<Source, Destination>(guid.ToString());
            _mapper.Build();
            var destination = _mapper.GetMapper<Source, Destination>(guid.ToString()).Map(new Source() { Value = 102, a = new A() { OtherValue = 103 }, c = new C() { OtherValue2 = 104 } });

            Assert.Equal(destination.OtherValue, 103);
        }

        [Fact]
        public void Map_With_Ignore()
        {
            Guid guid = Guid.NewGuid();
            _mapper.SetMapper<Source, Destination>(guid.ToString()).Ignore(x=>x.a);            
            _mapper.Build();
            var destination = _mapper.GetMapper<Source, Destination>(guid.ToString()).Map(new Source() { Value = 102, a = new A() { OtherValue = 103 }, c= new C() { OtherValue2 = 104 } });

            Assert.Equal(destination.OtherValue, 0);
        }

        [Fact]
        public void Map_With_Ignore_Set_Ignored_Member_Null()
        {
            Guid guid = Guid.NewGuid();
            _mapper.SetMapper<Source, Destination>(guid.ToString()).Ignore(x => x.a);            
            _mapper.Build();
            var destination = _mapper.GetMapper<Source, Destination>(guid.ToString()).Map(new Source() { Value = 102, a = null , c = new C() { OtherValue2 = 104 } });

            Assert.Equal(destination.OtherValue, 0);
        }

        [Fact]
        public void Map_With_Ignore_Derived_Member()
        {
            Guid guid = Guid.NewGuid();
            _mapper.SetMapper<Source, Destination>(guid.ToString()).Ignore(x => x.c);            
            _mapper.Build();
            var destination = _mapper.GetMapper<Source, Destination>(guid.ToString()).Map(new Source() { Value = 102, a= new A() { OtherValue = 102 },  c = new C() {  OtherValue2 = 103 } });

            Assert.Equal(destination.OtherValue, 102);
            Assert.Equal(destination.OtherValue2, 0);
        }      

    }
}
