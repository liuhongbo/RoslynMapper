using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using RoslynMapper.Convert;

namespace RoslynMapper.Map
{
    public class TypeMap<T1,T2> : ITypeMap<T1,T2>
    {
        private ITypeConvert _typeConvert = new TypeConvert();
        static private MapKey _mapKey = null;
        private Members _members = null;
        public TypeMap(): this(null)
        {

        }

        public TypeMap(string name)
        {            
            Name = name;
            _members = new Members();
        }
        public string Name { get; set; }

        public MapKey Key
        {
            get
            {
                return _mapKey??new MapKey(typeof(T1), typeof(T2), Name);
            }
        }

        public Type SourceType
        {
            get
            {
                return typeof(T1);
            }
        }

        public Type DestinationType
        {
            get
            {
                return typeof(T2);
            }
        }

        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }

        public IMembers Members
        {
            get
            {
                return _members;
            }
        }

        public string MapperName
        {
            get
            {
                return GetClassName();
            }
        }

        public Action<T1, T2> Resolver { get; set; }

        public string CreateMapper()
        {
            string code = string.Empty;

            code = string.Format(@"
    class {0}:{1}
    {{
        private IMapEngine _engine;
        private ITypeMap _typeMap;
        public {0}(IMapEngine engine, ITypeMap typeMap)
        {{
            _engine = engine;
            _typeMap = typeMap;
        }}

        public {3} Map({2} t1)
        {{
            return Map(t1, new {3}());
        }}

        public {3} Map({2} t1, {3} t2)
        {{
{4}
            return t2;
        }}

        public override int GetHashCode()
        {{
            return {5};
        }}

        public override IMapEngine MapEngine
        {{
            get
            {{
                return  _engine;
            }}
        }}

        public override ITypeMap<{2},{3}> TypeMap
        {{
            get
            {{
                return (ITypeMap<{2},{3}>)_typeMap;
            }}
        }}
    }}
", GetClassName(), GetBaseTypeName(), GetTypeFullName(SourceType), GetTypeFullName(DestinationType), GetMapBody("            "), GetHashCode(), Name);

            return code;
        }

        protected string GetClassName()
        {
            var srcName = SourceType.FullName;
            var destName = DestinationType.FullName;
            return string.Format("{0}__Map__{1}__{2}", srcName.Replace('.', '_').Replace('+', '_'), destName.Replace('.', '_').Replace('+', '_'), (Name ?? string.Empty).Replace('-', '_'));
        }

        protected string GetBaseTypeName()
        {
            return string.Format("MapperBase<{0},{1}>,IMapper<{0},{1}>", GetTypeFullName(SourceType), GetTypeFullName(DestinationType));
        }

        protected string GetMapBody(string indent)
        {
            string code = string.Empty;

            Type sourceType = SourceType;
            Type destinationType = DestinationType;

            if (Resolver != null)
            {
                code = string.Format("{0}Resolve(t1, t2);\r\n", indent);
                return code;
            }

            //var  bindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty;
            //var sourceMembers = sourceType.GetMembers(bindingFlags);
            //var destMembers = destinationType.GetMembers(bindingFlags);
            var sourceMemberInfos = ((MemberInfo[])sourceType.GetProperties()).Concat((MemberInfo[])sourceType.GetFields());
            var destMemberInfos = ((MemberInfo[])destinationType.GetProperties()).Concat((MemberInfo[])destinationType.GetFields());           

            MemberPath path = new MemberPath(destinationType, string.Empty);
            foreach (var destMemberInfo in destMemberInfos)
            {
                IMember<T1,T2> destMember = Members.GetMember<T1,T2>(new MemberKey(destMemberInfo, path));
                if (destMember != null)
                {
                    if (destMember.Ignored) continue;
                }
                else
                {
                    destMember = new Member<T1,T2>(destMemberInfo, path);
                    //Members.AddMember(destMember);
                }
                IMember<T1,T2> sourceMember = null;
                if (destMember.BindMember != null)
                {
                    sourceMember = destMember.BindMember;
                }
                else if (destMember.Resolver != null)
                {
                    code += GetMemberMapBody(destMember, indent);
                    continue;
                }
                else
                {
                    if (sourceMemberInfos.Any(m => m.Name == destMemberInfo.Name))
                    {
                        var sourceMemberInfo = sourceMemberInfos.First(m => m.Name == destMemberInfo.Name);
                        sourceMember = new Member<T1,T2>(sourceMemberInfo, new MemberPath(sourceType, string.Empty));

                    }
                }

                if (sourceMember != null)
                {
                    code += GetMemberMapBody(sourceMember, destMember, indent);
                }
            }

            return code;
        }

        private Type GetMemberType(MemberInfo member)
        {
            if (member is FieldInfo)
            {
                return (member as FieldInfo).FieldType;
            }

            if (member is PropertyInfo)
            {
                return (member as PropertyInfo).PropertyType;
            }

            return null;
        }

        private string GetMemberMapBody(IMember<T1, T2> destinationMember, string indent)
        {
            string code = string.Empty;
            if (destinationMember.Resolver != null)
            {
                code = string.Format("{0}MemberResolve(\"{1}\", t1, t2);\r\n", indent, destinationMember.Id);
            }
            return code;
        }

        private string GetMemberMapBody(IMember<T1, T2> sourceMember, IMember<T1, T2> destinationMember, string indent)
        {
            MemberInfo sourceMemberInfo = sourceMember.MemberInfo;
            MemberInfo destinationMemberInfo = destinationMember.MemberInfo;

            string code = string.Empty;
            Type sourceType = GetMemberType(sourceMemberInfo);
            Type destinationType = GetMemberType(destinationMemberInfo);

            if ((sourceType == destinationType) ||
                (destinationType == typeof(System.Object)) ||
                (destinationType.IsAssignableFrom(sourceType)) ||
                (_typeConvert.CanImplicitConvert(sourceType, destinationType)))
            {
                code = string.Format("{0}t2.{1}=t1.{2};\r\n", indent, GetMemberFullPathName(destinationMember), GetMemberFullPathName(sourceMember));
            }
            else if (_typeConvert.CanExplicitConvert(sourceType, destinationType))
            {
                code = string.Format("{0}t2.{1}=({2})t1.{3};\r\n", indent, GetMemberFullPathName(destinationMember), GetTypeFullName(GetMemberType(destinationMemberInfo)), GetMemberFullPathName(sourceMember));
            }
            else if (destinationType.IsEnum)
            {
                code = string.Format("{0}t2.{1}=({2})Enum.ToObject(typeof({2}),t1.{3});\r\n", indent, GetMemberFullPathName(destinationMember), GetTypeFullName(GetMemberType(destinationMemberInfo)), GetMemberFullPathName(sourceMember));
            }
            else if (_typeConvert.CanIConvertibleConvert(sourceType, destinationType))
            {
                code = string.Format("{0}t2.{1}=({2})System.Convert.ChangeType(t1.{3},typeof({2}));\r\n", indent, GetMemberFullPathName(destinationMember), GetTypeFullName(GetMemberType(destinationMemberInfo)), GetMemberFullPathName(sourceMember));
            }
            else if (_typeConvert.CanTypeConverterConvertFrom(sourceType, destinationType))
            {
                code = string.Format("{0}t2.{1}=({2})TypeDescriptor.GetConverter(typeof({2})).ConvertFrom(t1.{3});\r\n", indent, GetMemberFullPathName(destinationMember), GetTypeFullName(GetMemberType(destinationMemberInfo)), GetMemberFullPathName(sourceMember));
            }
            else if (_typeConvert.CanTypeConverterConvertTo(sourceType, destinationType))
            {
                code = string.Format("{0}t2.{1}=({2})TypeDescriptor.GetConverter(typeof({3})).ConvertTo(t1.{4},typeof({2}));\r\n", indent, GetMemberFullPathName(destinationMember), GetTypeFullName(GetMemberType(destinationMemberInfo)), GetTypeFullName(GetMemberType(sourceMemberInfo)), GetMemberFullPathName(sourceMember));
            }
            return code;
        }

        private string GetMemberFullPathName(IMember<T1, T2> member)
        {
            if (string.IsNullOrEmpty(member.Path.AccessPath))
            {
                return member.MemberInfo.Name;
            }
            else
            {
                return member.Path.AccessPath + "." + member.MemberInfo.Name;
            }
        }

        /// <summary>
        /// replace + with . for inner types
        /// </summary>
        /// <param name="type">Type Object</param>
        /// <returns></returns>
        private string GetTypeFullName(Type type)
        {
            return type.FullName.Replace('+', '.');
        }
    
    }
}
