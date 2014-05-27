using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using RoslynMapper.Convert;

namespace RoslynMapper.UnitTests.Convert
{
    public class TypeConvertTests
    {
        [Fact]
        public void CanNotImplicitConvert_Int32_to_Int16()
        {
            ITypeConvert convert = new TypeConvert();
            var ret = convert.CanImplicitConvert(typeof(Int32), typeof(Int16));

            Assert.False(ret);
        }

        [Fact]
        public void CanImplicitConvert_Int16_to_Int32()
        {
            ITypeConvert convert = new TypeConvert();
            var ret = convert.CanImplicitConvert(typeof(Int16), typeof(Int32));
           
            Assert.True(ret);
        }

        [Fact]
        public void CanNotImplicitConvert_Nullable_Decimal_to_Decimal()
        {
            ITypeConvert convert = new TypeConvert();
            var ret = convert.CanImplicitConvert(typeof(decimal?), typeof(decimal));

            Assert.False(ret);
        }

        [Fact]
        public void CanImplicitConvert_Decimal_to_Nullable_Decimal()
        {
            ITypeConvert convert = new TypeConvert();
            var ret = convert.CanImplicitConvert(typeof(decimal), typeof(decimal?));

            Assert.True(ret);
        }
    }
}
