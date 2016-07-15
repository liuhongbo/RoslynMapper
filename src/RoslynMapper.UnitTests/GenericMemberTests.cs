using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RoslynMapper.UnitTests
{
    public class GenericMemberTests : IClassFixture<MapEngineFixture>
    {
        private IMapEngine _mapper;
        public GenericMemberTests(MapEngineFixture fixture)
        {
            _mapper = fixture.Engine;
        }

        public class Source
        {
            public Source()
            {
                i = new List<int>();
            }

            public IList<int> i;            
        }

        public class Destination
        {
            public Destination()
            {                
                i = new List<float>();
            }
            public IList<float> i;           
        }

        [Fact]
        public void Map_Generic_Int_List_Member()
        {
            Guid guid = Guid.NewGuid();
            _mapper.SetMapper<Source, Destination>(guid.ToString());
            var ret = _mapper.Build();
            Assert.True(ret);

            var source = new Source() { };
            source.i.Add(5);
            var destination = _mapper.GetMapper<Source, Destination>(guid.ToString()).Map(source);
            Assert.True(destination.i.Count == 1);
            Assert.Equal(destination.i[0], 5);
        }

    }
}
