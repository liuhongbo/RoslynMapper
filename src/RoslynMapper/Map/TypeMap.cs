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
        private int _maxDepth = DEFAULT_MAX_DEPTH;
        private const int DEFAULT_MAX_DEPTH = 10;

        public TypeMap(): this(null)
        {

        }

        public TypeMap(string name) : this(name, DEFAULT_MAX_DEPTH)
        {
            
        }

        public TypeMap(string name, int maxDepth)
        {
            Name = name;
            _members = new Members();
            _maxDepth = maxDepth;
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

        public Func<IMember, string> CodeResolver { get; set; } 

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
", GetClassName(), GetBaseTypeName(), NormalizedTypeFullName(SourceType), NormalizedTypeFullName(DestinationType), GetMapBody("            "), GetHashCode(), Name);

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
            return string.Format("MapperBase<{0},{1}>,IMapper<{0},{1}>", NormalizedTypeFullName(SourceType), NormalizedTypeFullName(DestinationType));
        }


        protected void BuildMembers(Type type, MemberPath path, bool includeMethod , bool includeCanReadProperty, bool includeCanWriteProperty, int depth)
        {
            if (depth > _maxDepth) return;

            var memberInfos = GetMemberInfos(type, includeMethod, includeCanReadProperty, includeCanWriteProperty);

            foreach (var memberInfo in memberInfos)
            {
                if (memberInfo.DeclaringType.IsGenericTypeOf(typeof(IList<>))) continue;
                if (memberInfo.DeclaringType.IsGenericTypeOf(typeof(IEnumerable<>))) continue;
                if (memberInfo.DeclaringType.IsGenericTypeOf(typeof(ICollection<>))) continue;
                if (memberInfo.DeclaringType.IsGenericTypeOf(typeof(ISet<>))) continue;
                if (memberInfo.DeclaringType.IsGenericTypeOf(typeof(IDictionary<,>))) continue;

                IMember<T1, T2> member = Members.GetMember<T1, T2>(new MemberKey(memberInfo, path));
                if (member == null)
                {
                    member = new Member<T1, T2>(memberInfo, path);
                    Members.AddMember(member);
                }

                Type memberType = memberInfo.GetMemberType();

                if (!member.Ignored)
                {
                    if ( !(memberInfo is MethodInfo)
                        && (!TypeConvert.IsPrimitiveType(memberType)) 
                        && (!memberType.IsEnum)
                        && (!(memberType == typeof(Guid)))
                        && (!(memberType == typeof(object))))
                    { 
                        var memberPath = new MemberPath(path.RootType, path.AccessPath + (string.IsNullOrEmpty(path.AccessPath) ? "" : ".") + memberInfo.Name);
                        BuildMembers(memberType, memberPath, includeMethod, includeCanReadProperty, includeCanWriteProperty, depth + 1);
                    }
                }
            }
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

            BuildMembers(sourceType, new MemberPath(sourceType, string.Empty), true, true, false, 0);
            BuildMembers(destinationType, new MemberPath(destinationType, string.Empty), false, false, true, 0);            

            code += GetMemberMapBody(null, new MemberPath(DestinationType, string.Empty), null, indent);

            return code;
        }


        private IMember<T1, T2> GetBindSourceMember(IMember<T1, T2> destMember, IMember<T1,T2> binder)
        {
            if (destMember.BindMember != null) return destMember.BindMember;

            IMember<T1, T2> srcMember = null;

            if (binder != null)
            {
                var binderPath = new MemberPath(SourceType,binder.FullName);
                srcMember = Members.GetMembers<T1, T2>(SourceType, binderPath)
                    .Where(m => m.MemberInfo.Name == destMember.MemberInfo.Name)
                    .FirstOrDefault();
                if (srcMember != null)
                {
                    return srcMember;
                }
                else
                {
                    srcMember = Members.GetMembers<T1, T2>(SourceType).Where(m => m.Path <= binderPath).FirstOrDefault();
                    if (srcMember != null)
                    {
                        return srcMember;
                    }
                }
            }

            srcMember = Members.GetMembers<T1, T2>(SourceType, new MemberPath(SourceType, destMember.Path.AccessPath)).Where(m => m.MemberInfo.Name == destMember.MemberInfo.Name).FirstOrDefault();
            if (srcMember != null) return srcMember;

            return Members.GetMembers<T1, T2>(SourceType).Where(m => m.MemberInfo.Name == destMember.MemberInfo.Name).FirstOrDefault();
        }

        private IEnumerable<MemberInfo> GetMemberInfos(Type type, bool includeMethod, bool includeCanReadProperty, bool includeCanWriteProperty)
        {
            var memberInfos = ((IEnumerable<MemberInfo>)type.GetProperties().Where(p => (p.CanRead && includeCanReadProperty) || (p.CanWrite && includeCanWriteProperty))).Concat((MemberInfo[])type.GetFields());

            if (includeMethod)
            {
                memberInfos = memberInfos.Concat((IEnumerable<MemberInfo>)type.GetMethods().Where(m => (m.GetParameters().Length == 0) && (m.ReturnType != typeof(void)) && (!m.IsSpecialName)));
            }

            return memberInfos;
        }        

        private string GetMemberMapBody(IMember<T1, T2> destMember, MemberPath path, IMember<T1,T2> binder, string indent)
        {
            string code = string.Empty;
            if (destMember == null) //root
            {
                foreach (var m in Members.GetMembers<T1, T2>(DestinationType, path))
                {
                    code += GetMemberMapBody(m, path, null, indent);
                }
            }
            else
            {
                if (destMember.Ignored) { return code; }                

                if ((destMember.Resolver != null) ||
                    (CodeResolver != null) || 
                    (destMember.CodeResolver != null))
                {
                    code += GetMemberMapBody(destMember, indent);
                    return code;
                }
                IMember<T1, T2> sourceMember = null;
                if (destMember.BindMember != null)
                {
                    sourceMember = destMember.BindMember;
                    binder = sourceMember;
                }
                else
                {
                    sourceMember = GetBindSourceMember(destMember, binder);
                }

                if (sourceMember != null)
                {
                    var c = GetMemberMapBody(sourceMember, destMember, indent);
                    if (string.IsNullOrEmpty(c))
                    {
                        var destMemberType = destMember.MemberInfo.GetMemberType();
                        string destMemberTypeString = null;
                        if (destMemberType.IsConcreteType())
                        {                            
                            destMemberTypeString = NormalizedTypeFullName(destMemberType);
                        }
                        else if (destMemberType.IsInterface)
                        {
                            if (destMemberType.IsGenericTypeOf(typeof(IList<>)))
                            {
                                //destMemberTypeString = NormalizedTypeFullName(typeof(List<>).MakeGenericType(destMemberType.GenericTypeArguments));
                                destMemberTypeString = TypeSyntaxFactory.GetTypeSyntax("List", destMemberType.GenericTypeArguments[0].FullName).ToFullString();
                                
                            }
                            else if (destMemberType.IsGenericTypeOf(typeof(ICollection<>)))
                            {
                                //destMemberTypeString = NormalizedTypeFullName(typeof(List<>).MakeGenericType(destMemberType.GenericTypeArguments));
                                destMemberTypeString = TypeSyntaxFactory.GetTypeSyntax("List", destMemberType.GenericTypeArguments[0].FullName).ToFullString();
                            }
                            //more ???
                        }
                        if (!string.IsNullOrEmpty(destMemberTypeString))
                        {
                            code += string.Format("{0}if (t2.{1} == null) t2.{1} = new {2}();\r\n", indent, destMember.GetMemberFullPathName(), destMemberTypeString);                            
                        }
                        var memberPath = new MemberPath(path.RootType, path.AccessPath + (string.IsNullOrEmpty(path.AccessPath) ? "" : ".") + destMember.MemberInfo.Name);
                        foreach (var m in Members.GetMembers<T1, T2>(DestinationType, memberPath))
                        {
                            code += GetMemberMapBody(m, memberPath, binder, indent);
                        }
                    }
                    else
                    {
                        code += c;
                    }
                }
            }

            return code;
        }        

        private string GetMemberMapBody(IMember<T1, T2> destinationMember, string indent)
        {
            string code = string.Empty;
            if (destinationMember.Resolver != null)
            {
                code = string.Format("{0}MemberResolve(\"{1}\", t1, t2);\r\n", indent, destinationMember.Id);
            }
            else if (destinationMember.CodeResolver != null)
            {
                code = string.Format("{0}{1}\r\n", indent, destinationMember.CodeResolver());
            }
            else if (this.CodeResolver != null)
            {
                code = string.Format("{0}{1}\r\n", indent, CodeResolver(destinationMember));
            }
            return code;
        }

        private string GetMemberMapBody(IMember<T1, T2> sourceMember, IMember<T1, T2> destinationMember, string indent)
        {
            MemberInfo sourceMemberInfo = sourceMember.MemberInfo;
            MemberInfo destinationMemberInfo = destinationMember.MemberInfo;

            string code = string.Empty;
            Type sourceType = sourceMemberInfo.GetMemberType();
            Type destinationType = destinationMemberInfo.GetMemberType();

            if ((sourceType == destinationType) ||
                (destinationType == typeof(System.Object)) ||
                (destinationType.IsAssignableFrom(sourceType)) ||
                (_typeConvert.CanImplicitConvert(sourceType, destinationType)))
            {
                code = string.Format("{0}t2.{1} = t1.{2};\r\n", indent, destinationMember.GetMemberFullPathName(), sourceMember.GetMemberFullPathName());
            }
            else if (_typeConvert.CanExplicitConvert(sourceType, destinationType))
            {
                code = string.Format("{0}t2.{1} = ({2})t1.{3};\r\n", indent, destinationMember.GetMemberFullPathName(), NormalizedTypeFullName(destinationMemberInfo.GetMemberType()), sourceMember.GetMemberFullPathName());
            }
            else if (destinationType.IsEnum)
            {
                code = string.Format("{0}t2.{1} = ({2})Enum.ToObject(typeof({2}),t1.{3});\r\n", indent, destinationMember.GetMemberFullPathName(), NormalizedTypeFullName(destinationMemberInfo.GetMemberType()), sourceMember.GetMemberFullPathName());
            }
            else if (_typeConvert.CanIConvertibleConvert(sourceType, destinationType))
            {
                code = string.Format("{0}t2.{1} = ({2})System.Convert.ChangeType(t1.{3},typeof({2}));\r\n", indent, destinationMember.GetMemberFullPathName(), NormalizedTypeFullName(destinationMemberInfo.GetMemberType()), sourceMember.GetMemberFullPathName());
            }
            else if (_typeConvert.CanTypeConverterConvertFrom(sourceType, destinationType))
            {
                code = string.Format("{0}t2.{1} = ({2})TypeDescriptor.GetConverter(typeof({2})).ConvertFrom(t1.{3});\r\n", indent, destinationMember.GetMemberFullPathName(), NormalizedTypeFullName(destinationMemberInfo.GetMemberType()), sourceMember.GetMemberFullPathName());
            }
            else if (_typeConvert.CanTypeConverterConvertTo(sourceType, destinationType))
            {
                code = string.Format("{0}t2.{1} = ({2})TypeDescriptor.GetConverter(typeof({3})).ConvertTo(t1.{4},typeof({2}));\r\n", indent, destinationMember.GetMemberFullPathName(), NormalizedTypeFullName(destinationMemberInfo.GetMemberType()), NormalizedTypeFullName(sourceMemberInfo.GetMemberType()), sourceMember.GetMemberFullPathName());
            }
            return code;
        }        

        /// <summary>
        /// replace + with . for inner types
        /// </summary>
        /// <param name="type">Type Object</param>
        /// <returns></returns>
        private string NormalizedTypeFullName(Type type)
        {
            return type.FullName.Replace('+', '.');
        }        
    }
}
