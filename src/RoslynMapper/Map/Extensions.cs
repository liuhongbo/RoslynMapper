using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RoslynMapper.Map
{
    public static class Extensions
    {
        public static Type GetMemberType(this MemberInfo member)
        {
            if (member is FieldInfo)
            {
                return (member as FieldInfo).FieldType;
            }
            else if (member is PropertyInfo)
            {
                return (member as PropertyInfo).PropertyType;
            }
            else if (member is MethodInfo)
            {
                return (member as MethodInfo).ReturnType;
            }

            return null;
        }

        public static string GetMemberFullPathName(this IMember member)
        {
            if (string.IsNullOrEmpty(member.Path.AccessPath))
            {
                if (member.MemberInfo is MethodInfo)
                {
                    return member.MemberInfo.Name + "()";
                }
                else
                {
                    return member.MemberInfo.Name;
                }
            }
            else
            {
                if (member.MemberInfo is MethodInfo)
                {
                    return member.Path.AccessPath + "." + member.MemberInfo.Name + "()";
                }
                else
                {
                    return member.Path.AccessPath + "." + member.MemberInfo.Name;
                }
            }
        }

        public static bool IsConcreteType(this Type type)
        {
            return !type.IsAbstract && !type.IsInterface;
        }

        public static bool IsGenericTypeOf(this Type type, Type genericType)
        {
            return ((type.IsGenericType) && type.GetGenericTypeDefinition() == genericType);
        }
    }
}
