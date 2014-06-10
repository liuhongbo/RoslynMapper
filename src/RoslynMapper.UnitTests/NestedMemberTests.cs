﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RoslynMapper.UnitTests
{
    public class OneLayerNestedMemberTests : IClassFixture<MapEngineFixture>
    {
        private IMapEngine _mapper;
        public OneLayerNestedMemberTests(MapEngineFixture fixture)
        {
            _mapper = fixture.Engine;
        }

        public class Source
        {
            public class Int1
            {
                public int i = 10;
            }

            public Int1 i1 = new Int1();
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
        public void Map_One_Layer_Nested_Member()
        {
            Guid guid = Guid.NewGuid();
            _mapper.SetMapper<Source, Destination>(guid.ToString());
            _mapper.Build();

            var destination = _mapper.GetMapper<Source, Destination>(guid.ToString()).Map(new Source() { });
            Assert.Equal(destination.i1.i, 10);

        }
    }

    public class TwoLayerNestedMemberTests: IClassFixture<MapEngineFixture>
    {
        private IMapEngine _mapper;
        public TwoLayerNestedMemberTests(MapEngineFixture fixture)
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
                public Int1 i1 = new Int1();                
            }

            public Int2 i1 = new Int2();
        }

        public class Destination
        {
            public class Int1
            {              
                public int i;
            }

            public class Int2
            {
                public Int1 i1;
            }

            public Int2 i1 { get; set; }
        }

        [Fact]
        public void Map_Two_Layer_Nested_Member()
        {
            Guid guid = Guid.NewGuid();
            _mapper.SetMapper<Source, Destination>(guid.ToString());
            _mapper.Build();

            var destination = _mapper.GetMapper<Source, Destination>(guid.ToString()).Map(new Source() {});
            Assert.Equal(destination.i1.i1.i, 10);

        }
    }
}
