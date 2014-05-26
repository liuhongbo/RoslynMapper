using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RoslynMapper.Convert
{
    public class TypeConvert : ITypeConvert
    {

        public bool CanConvert(Type sourceType, Type destinationType)
        {
            throw new Exception("not implemented");
        }

        public bool CanConvert(object value, Type destinationType)
        {
            throw new Exception("not implemented");
        }

        public bool TryConvert(object value, Type destinationType, out object result)
        {
            throw new Exception("not implemented");
        }

        public object Convert(object value, Type destinationType)
        {
            throw new Exception("not implemented");
        }

        public bool CanImplicitConvert(Type sourceType, Type destinationType)
        {
            if ((GetImplicitConversionOperator(sourceType, destinationType, sourceType) != null) ||
                (GetImplicitConversionOperator(sourceType, destinationType, destinationType) != null))
            {
                return true;
            }
            else
            {
                //http://msdn.microsoft.com/en-us/library/y5b434w4.aspx
                if (IsNumericType(sourceType) && IsNumericType(destinationType))
                {
                    var destTypeCode = Type.GetTypeCode(destinationType);
                    switch (Type.GetTypeCode(sourceType))
                    {
                        case TypeCode.SByte:
                            return ((destTypeCode == TypeCode.Int16) ||
                                    (destTypeCode == TypeCode.Int32) ||
                                    (destTypeCode == TypeCode.Int64) ||
                                    (destTypeCode == TypeCode.Single) ||
                                    (destTypeCode == TypeCode.Double) ||
                                    (destTypeCode == TypeCode.Decimal));                            
                        case TypeCode.Byte:
                            return ((destTypeCode == TypeCode.Int16) ||
                                    (destTypeCode == TypeCode.UInt16) ||
                                    (destTypeCode == TypeCode.Int32) ||
                                    (destTypeCode == TypeCode.UInt32) ||
                                    (destTypeCode == TypeCode.Int64) ||
                                    (destTypeCode == TypeCode.UInt64) ||
                                    (destTypeCode == TypeCode.Single) ||
                                    (destTypeCode == TypeCode.Double) ||
                                    (destTypeCode == TypeCode.Decimal)); 
                        case TypeCode.Int16:
                            return ((destTypeCode == TypeCode.Int32) ||
                                    (destTypeCode == TypeCode.Int64) ||
                                    (destTypeCode == TypeCode.Single) ||
                                    (destTypeCode == TypeCode.Double) ||
                                    (destTypeCode == TypeCode.Decimal)); 
                        case TypeCode.UInt16:
                            return ((destTypeCode == TypeCode.Int32) ||
                                    (destTypeCode == TypeCode.UInt32) ||
                                    (destTypeCode == TypeCode.Int64) ||
                                    (destTypeCode == TypeCode.UInt64) ||
                                    (destTypeCode == TypeCode.Single) ||
                                    (destTypeCode == TypeCode.Double) ||
                                    (destTypeCode == TypeCode.Decimal)); 
                        case TypeCode.Int32:
                            return ((destTypeCode == TypeCode.Int64) ||
                                    (destTypeCode == TypeCode.Single) ||
                                    (destTypeCode == TypeCode.Double) ||
                                    (destTypeCode == TypeCode.Decimal)); 
                        case TypeCode.UInt32:
                            return ((destTypeCode == TypeCode.Int64) ||
                                    (destTypeCode == TypeCode.UInt64) ||
                                    (destTypeCode == TypeCode.Single) ||
                                    (destTypeCode == TypeCode.Double) ||
                                    (destTypeCode == TypeCode.Decimal)); 
                        case TypeCode.Int64:
                            return ((destTypeCode == TypeCode.Single) ||
                                    (destTypeCode == TypeCode.Double) ||
                                    (destTypeCode == TypeCode.Decimal)); 
                        case TypeCode.UInt64:
                            return ((destTypeCode == TypeCode.Single) ||
                                    (destTypeCode == TypeCode.Double) ||
                                    (destTypeCode == TypeCode.Decimal)); 
                        case TypeCode.Char:
                            return ((destTypeCode == TypeCode.UInt16) ||
                                    (destTypeCode == TypeCode.Int32) ||
                                    (destTypeCode == TypeCode.UInt32) ||
                                    (destTypeCode == TypeCode.Int64) ||
                                    (destTypeCode == TypeCode.UInt64) ||
                                    (destTypeCode == TypeCode.Single) ||
                                    (destTypeCode == TypeCode.Double) ||
                                    (destTypeCode == TypeCode.Decimal)); 
                        case TypeCode.Single:
                            return (destTypeCode == TypeCode.Double);
                        case TypeCode.Double:
                            break;

                    }
                }
            }
            return false;
        }

        private MethodInfo GetImplicitConversionOperator(Type sourceType, Type destinationType,Type invokerType)
        {
            return GetConversionOperator(sourceType, destinationType,invokerType, "op_Implicit");
        }

        private MethodInfo GetExplicitConversionOperator(Type sourceType, Type destinationType, Type invokerType)
        {
            return GetConversionOperator(sourceType, destinationType, invokerType, "op_Explicit");
        }

        private MethodInfo GetConversionOperator(Type sourceType, Type destinationType, Type invokerType, string operatorMethodName)
        {
            foreach (var method in invokerType.GetMethods(BindingFlags.Public | BindingFlags.Static).Where(m => m.Name == operatorMethodName))
            {
                if (destinationType.IsAssignableFrom(method.ReturnType))
                {
                    var parameters = method.GetParameters();
                    if (parameters.Count() == 1 && parameters[0].ParameterType == sourceType)
                    {
                        return method;
                    }
                }
            }

            return null;
        }

        public bool CanImplicitConvert(object value, Type destinationType)
        {
            throw new Exception("not implemented");
        }

        public bool TryImplicitConvert(object value, Type destinationType, out object result)
        {
            throw new Exception("not implemented");
        }

        public object ImplicitConvert(object value, Type destinationType)
        {
            throw new Exception("not implemented");
        }

        public bool CanExplicitConvert(Type sourceType, Type destinationType)
        {
            if ((GetExplicitConversionOperator(sourceType, destinationType, sourceType) != null) ||
                (GetExplicitConversionOperator(sourceType, destinationType, destinationType) != null))
            {
                return true;
            }

            return false;
        }

        public bool CanExplicitConvert(object value, Type destinationType)
        {
            throw new Exception("not implemented");
        }

        public bool TryExplicitConvert(object value, Type destinationType, out object result)
        {
            throw new Exception("not implemented");
        }

        public object ExplicitConvert(object value, Type destinationType)
        {
            throw new Exception("not implemented");
        }

        public bool CanIConvertibleConvert(Type sourceType, Type destinationType)
        {
            throw new Exception("not implemented");
        }

        public bool CanIConvertibleConvert(object value, Type destinationType)
        {
            throw new Exception("not implemented");
        }

        public bool TryIConvertibleConvert(object value, Type destinationType, out object result)
        {
            throw new Exception("not implemented");
        }

        public object IConvertibleConvert(object value, Type destinationType)
        {
            throw new Exception("not implemented");
        }

        public bool CanTypeConverterConvert(Type sourceType, Type destinationType)
        {
            throw new Exception("not implemented");
        }

        public bool CanTypeConverterConvert(object value, Type destinationType)
        {
            throw new Exception("not implemented");
        }

        public bool TryTypeConverterCOnvert(object value, Type destinationType, out object result)
        {
            throw new Exception("not implemented");
        }

        public object TypeConverterConvert(object value, Type destinationType)
        {
            throw new Exception("not implemented");
        }

        #region Helper

        protected static bool IsNumericType(Type type)
        {
            return ((type == typeof(sbyte)) ||
                   (type == typeof(byte)) ||
                   (type == typeof(short)) ||
                   (type == typeof(ushort)) ||
                   (type == typeof(int)) ||
                   (type == typeof(uint)) ||
                   (type == typeof(long)) ||
                   (type == typeof(ulong)) ||
                   (type == typeof(char)) ||
                   (type == typeof(float)) ||
                   (type == typeof(double)) ||
                   (type == typeof(decimal)));
        }

        protected static bool IsSimpleType(Type type)
        {
            return (IsNumericType(type) ||
                    (type == typeof(bool)));
        }

        protected static bool IsBuildInType(Type type)
        {
            return (IsSimpleType(type) ||
                    (type == typeof(object)) ||
                    (type == typeof(string)));
        }       

        #endregion
    }

    //public class TypeConvert<T1, T2> : ITypeConvert<T1, T2>
    //{

    //}
}
