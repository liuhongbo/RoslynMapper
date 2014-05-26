using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynMapper.Convert
{
    public interface ITypeConvert
    {
        bool CanConvert(Type sourceType, Type destinationType);
        bool CanConvert(object value, Type destinationType);
        bool TryConvert(object value, Type destinationType, out object result);
        object Convert(object value, Type destinationType);

        bool CanImplicitConvert(Type sourceType, Type destinationType);
        bool CanImplicitConvert(object value, Type destinationType);
        bool TryImplicitConvert(object value, Type destinationType, out object result);
        object ImplicitConvert(object value, Type destinationType);

        bool CanExplicitConvert(Type sourceType, Type destinationType);
        bool CanExplicitConvert(object value, Type destinationType);
        bool TryExplicitConvert(object value, Type destinationType, out object result);
        object ExplicitConvert(object value, Type destinationType);

        bool CanIConvertibleConvert(Type sourceType, Type destinationType);
        bool CanIConvertibleConvert(object value, Type destinationType);
        bool TryIConvertibleConvert(object value, Type destinationType, out object result);
        object IConvertibleConvert(object value, Type destinationType);

        bool CanTypeConverterConvert(Type sourceType, Type destinationType);
        bool CanTypeConverterConvert(object value, Type destinationType);
        bool TryTypeConverterCOnvert(object value, Type destinationType, out object result);
        object TypeConverterConvert(object value, Type destinationType);
    }

    public interface ITypeConvert<T1,T2> : ITypeConvert
    {
        bool CanConvert();
        bool CanConvert(T1 t1);
        bool TryConvert(T1 t1, out T2 t2);
        T2 Convert(T1 t1);

        bool CanImplicitConvert();
        bool CanImplicitConvert(T1 t1);
        bool TryImplicitConvert(T1 t1, out T2 t2);
        T2 ImplicitConvert(T1 t1);

        bool CanExplicitConvert();
        bool CanExplicitConvert(T1 t1);
        bool TryExplicitConvert(T1 t1, out T2 t2);
        T2 ExplicitConvert(T1 t1);

        bool CanIConvertibleConvert();
        bool CanIConvertibleConvert(T1 t1);
        bool TryIConvertibleConvert(T1 t1, out T2 t2);
        T2 IConvertibleConvert(T1 t1);

        bool CanTypeConverterConvert();
        bool CanTypeConverterConvert(T1 t1);
        bool TryTypeConverterCOnvert(T1 t1, out T2 t2);
        T2 TypeConverterConvert(T1 t1);
    }
}
