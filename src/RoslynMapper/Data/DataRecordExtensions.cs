using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using RoslynMapper.Map;

namespace RoslynMapper.Data
{
    public static class DataRecordExtensions
    {
        public static string DataRecordCodeResolver(IMember member)
        {
            string code = string.Empty;

            var type = member.MemberInfo.GetMemberType();
            string getFuncName = string.Empty;
            if (type == typeof(bool))
            {
                getFuncName = "GetBoolean";
            }
            else if (type == typeof(byte))
            {
                getFuncName = "GetByte";
            }
            else if (type == typeof(char))
            {
                getFuncName = "GetChar";
            }
            else if (type == typeof(DateTime))
            {
                getFuncName = "GetDateTime";
            }
            else if (type == typeof(decimal))
            {
                getFuncName = "GetDecimal";
            }
            else if (type == typeof(double))
            {
                getFuncName = "GetDouble";
            }
            else if (type == typeof(float))
            {
                getFuncName = "GetFloat";
            }
            else if (type == typeof(Guid))
            {
                getFuncName = "GetGuid";
            }
            else if (type == typeof(short))
            {
                getFuncName = "GetInt16";
            }
            else if (type == typeof(Int32))
            {
                getFuncName = "GetInt32";
            }
            else if (type == typeof(Int64))
            {
                getFuncName = "GetInt64";
            }
            else if (type == typeof(string))
            {
                getFuncName = "GetString";
            }

            if (!string.IsNullOrEmpty(getFuncName))
            {
                code = string.Format("t2.{0}=t1.{1}(t1.GetOrdinal(\"{2}\"));", member.GetMemberFullPathName(),getFuncName, member.MemberInfo.Name);
            }

            return code;
        }

        public static IMapping<IDataRecord, T> SetMapper<T>(this IDataRecord record, IMapEngine mapper, string name)
        {
            return mapper.SetMapper<IDataRecord, T>(name).CodeResolve(DataRecordCodeResolver);
        }

        public static IMapping<IDataRecord, T> SetMapper<T>(this IDataRecord record, IMapEngine mapper)
        {
            return SetMapper<T>(record, mapper, null);
        }
        
        public static T Get<T>(this IDataRecord record)
        {
            return Get<T>(record, MapEngine.DefaultInstance);
        }

        public static T Get<T>(this IDataRecord record, IMapEngine mapper)
        {
            return mapper.Map<IDataRecord, T>(record);
        }

        public static T Get<T>(this IDataRecord record, IMapEngine mapper, string name)
        {
            return mapper.Map<IDataRecord, T>(name, record);
        }

        public static T Get<T>(this IDataReader reader)
        {
            return Get<T>(reader, MapEngine.DefaultInstance);
        }

        public static T Get<T>(this IDataReader reader, IMapEngine mapper)
        {
            return Get<T>(reader, mapper, null);
        }

        public static T Get<T>(this IDataReader reader, IMapEngine mapper, string name)
        {
            return Get<T>((IDataRecord)reader, mapper, name);
        }
    }
}
