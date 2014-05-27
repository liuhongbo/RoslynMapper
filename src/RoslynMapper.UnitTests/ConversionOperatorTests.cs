using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RoslynMapper.UnitTests
{
    public class TypeMemberImplicitConversionOperator:IClassFixture<MapEngineFixture>
    {
         private IMapEngine _mapper = null;
         public TypeMemberImplicitConversionOperator(MapEngineFixture fixture)
        {
            _mapper = fixture.Engine;
        }

        public struct Digit
        {
             byte value;

             public Digit(byte value)  //constructor
             {
                 if (value > 9)
                 {
                     throw new System.ArgumentException();
                 }
                 this.value = value;
             }

             public static implicit operator byte(Digit d)  // implicit digit to byte conversion operator
             {
                 System.Console.WriteLine("conversion occurred");
                 return d.value;  // implicit conversion
             }
         }

         public class Source
         {
             public Digit value { get; set; }
         }

         public class Destination
         {
             public byte value { get; set; }
         }

         [Fact]
         public void Map_TypeMember_From_Digit_to_Int_Using_ImplicitConversionOperator()
         {
             Guid guid = Guid.NewGuid();
             _mapper.SetMapper<Source, Destination>(guid.ToString());
             _mapper.Build();
             var destination = _mapper.GetMapper<Source, Destination>(guid.ToString()).Map(new Source { value = new Digit(9) });

             Assert.Equal(destination.value, 9);
         }
    }

    public class TypeMemberExplicitConversionOperator : IClassFixture<MapEngineFixture>
    {
        private IMapEngine _mapper = null;
        public TypeMemberExplicitConversionOperator(MapEngineFixture fixture)
        {
            _mapper = fixture.Engine;
        }

        public struct Digit
        {
            byte value;

            public Digit(byte value)  //constructor
            {
                if (value > 9)
                {
                    throw new System.ArgumentException();
                }
                this.value = value;
            }

            public static explicit operator Digit(byte b)  // explicit byte to digit conversion operator
            {
                Digit d = new Digit(b);  // explicit conversion

                System.Console.WriteLine("Conversion occurred.");
                return d;
            }

            public override bool Equals(object obj)
            {
                return ((Digit)obj).value == value;
            }
        }

        public class Source
        {
            public byte value { get; set; }
        }

        public class Destination
        {
            public Digit value { get; set; }
        }

        [Fact]
        public void Map_TypeMember_From_Int_to_Digit_Using_ExplicitConversionOperator()
        {
            Guid guid = Guid.NewGuid();
            _mapper.SetMapper<Source, Destination>(guid.ToString());
            _mapper.Build();
            var destination = _mapper.GetMapper<Source, Destination>(guid.ToString()).Map(new Source { value = 7 });

            Assert.Equal(destination.value, new Digit(7));
        }
    }
}
