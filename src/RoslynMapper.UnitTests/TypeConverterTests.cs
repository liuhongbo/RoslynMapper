using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RoslynMapper.UnitTests
{
    public class TypeWithCustomTypeConverter :IClassFixture<MapEngineFixture>
    {
        private IMapEngine _mapper = null;
        public TypeWithCustomTypeConverter(MapEngineFixture fixture)
        {
            _mapper = fixture.Engine;
        }

        [TypeConverter(typeof(CustomTypeConverter))]
        public class Source
        {
            public int Value { get; set; }
        }

        public class Destination
        {
            public int OtherValue { get; set; }
        }

        public class CustomTypeConverter : TypeConverter
        {
            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                return destinationType == typeof(Destination);
            }

            public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
            {
                return new Destination
                {
                    OtherValue = ((Source)value).Value + 10
                };
            }
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                return sourceType == typeof(Destination);
            }
            public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
            {
                return new Source { Value = ((Destination)value).OtherValue - 10 };
            }
        }

        [Fact(Skip="check type converter on src/dest type not implemented yet")]
        public void Map_TypeWithCustomTypeConverter_UseCustomTypeConverterToMap()
        {
            Guid guid = Guid.NewGuid();
            _mapper.SetMapper<Source, Destination>(guid.ToString()).Bind(t1 => t1.Value, t2 => t2.OtherValue);
            _mapper.Build();
            var _destination = _mapper.GetMapper<Source, Destination>(guid.ToString()).Map(new Source { Value = 4});

            Assert.Equal(_destination.OtherValue, 14);
        }       

    }

    public class TypeMemberWithCustomTypeConverter : IClassFixture<MapEngineFixture>
    {
        private IMapEngine _mapper = null;
        public TypeMemberWithCustomTypeConverter(MapEngineFixture fixture)
        {
            _mapper = fixture.Engine;
        }

        public class Source
        {
            public Type1 value { get; set; }
        }

        public class Destination
        {
            public Type2 value { get; set; }
        }


        [TypeConverter(typeof(CustomTypeConverter))]
        public class Type1
        {
            public int Value { get; set; }
        }

        public class Type2
        {
            public int OtherValue { get; set; }
        }

        public class CustomTypeConverter : TypeConverter
        {
            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                return destinationType == typeof(Type2);
            }

            public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
            {
                return new Type2
                {
                    OtherValue = ((Type1)value).Value + 10
                };
            }
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                return sourceType == typeof(Type2);
            }
            public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
            {
                return new Type1 { Value = ((Type2)value).OtherValue - 10 };
            }
        }

        [Fact]
        public void Map_TypeMemberWithCustomTypeConverter_UseCustomTypeConverterToConvertTo()
        {
            Guid guid = Guid.NewGuid();
            _mapper.SetMapper<Source, Destination>(guid.ToString());
            _mapper.Build();
            var destination = _mapper.GetMapper<Source, Destination>(guid.ToString()).Map(new Source { value = new Type1 { Value = 4 } });

            Assert.Equal(destination.value.OtherValue, 14);
        }

        [Fact]
        public void Map_TypeMemberWithCustomTypeConverter_UseCustomTypeConverterToConvertFrom()
        {
            Guid guid = Guid.NewGuid();
            _mapper.SetMapper<Destination,Source>(guid.ToString());
            _mapper.Build();
            var converted = _mapper.GetMapper<Destination, Source>(guid.ToString()).Map(new Destination { value = new Type2 {  OtherValue = 4 } });

            Assert.Equal(converted.value.Value, -6);
        }
    }       
}
