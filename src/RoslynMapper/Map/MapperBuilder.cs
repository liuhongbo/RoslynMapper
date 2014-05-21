﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace RoslynMapper.Map
{
    public class MapperBuilder : IMapperBuilder
    {
        public IEnumerable<IMapper> Build(IEnumerable<ITypeMap> typeMaps)
        {
            var mapperList = new List<IMapper>();
            StringBuilder sb = new StringBuilder();
            
            foreach (var typeMap in typeMaps){
                sb.Append(GetClass(typeMap));                
            }

            if (sb.Length > 0)
            {
                sb.Insert(0, string.Format(@"
{0}

namespace {1}
{{", GetUsings(typeMaps), GetNamespace(typeMaps)));

                sb.Append("}");
            }

            Assembly assembly = BuildAssembly(sb.ToString(), typeMaps);

            if (assembly != null)
            {
                foreach (var typeMap in typeMaps)
                {
                    Type mapType = assembly.GetType(string.Format("{0}.{1}", GetNamespace(typeMaps), GetClassName(typeMap)));
                    if (mapType != null)
                    {
                        IMapper mapper = (IMapper)Activator.CreateInstance(mapType);
                        if (mapper != null)
                        {
                            mapperList.Add(mapper);
                        }
                    }                   
                }
            }

            //Console.WriteLine(sb.ToString());

            return mapperList;
        }


        public IMapper Build(ITypeMap typeMap)
        {
            IMapper mapper = null;

            var classCode = GetClass(typeMap);

            var code = string.Format(
@"{0}

namespace {1}
{{
    {2}
}}
", GetUsings(new ITypeMap[] { typeMap }), GetNamespace(new ITypeMap[] { typeMap }), classCode);

            Assembly assembly = BuildAssembly(code, new ITypeMap[]{ typeMap });

            if (assembly != null)
            {
                Type mapType = assembly.GetType(string.Format("{0}.{1}", GetNamespace(new ITypeMap[] { typeMap }), GetClassName(typeMap)));
                mapper = (IMapper)Activator.CreateInstance(mapType);
            }
            return mapper;
        }

        protected string GetNamespace(IEnumerable<ITypeMap> typeMaps)
        {
            return "RoslynMapper.Mappers";
        }

        protected string GetUsings(IEnumerable<ITypeMap> typeMaps)
        {
            return
@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;";
        }

        protected string GetClass(ITypeMap typeMap)
        {
            string code = string.Empty;
         
            code = string.Format(@"
    class {0}:{1}
    {{
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
    }}
", GetClassName(typeMap), GetBaseTypeName(typeMap), GetTypeFullName(typeMap.SourceType), GetTypeFullName(typeMap.DestinationType), GetMapBody(typeMap,"            "), typeMap.Key);

            return code;
        }

        protected string GetClassName(ITypeMap typeMap)
        {
            var srcName = typeMap.SourceType.FullName;
            var destName = typeMap.DestinationType.FullName;
            return string.Format("{0}__Map__{1}", srcName.Replace('.', '_').Replace('+','_'), destName.Replace('.', '_').Replace('+','_'));
        }

        protected string GetBaseTypeName(ITypeMap typeMap)
        {            
            return string.Format("IMapper<{0},{1}>", GetTypeFullName(typeMap.SourceType), GetTypeFullName(typeMap.DestinationType));
        }

        protected string GetMapBody(ITypeMap typeMap, string indent)
        {
            string code = string.Empty;

            Type sourceType = typeMap.SourceType;
            Type destinationType = typeMap.DestinationType;

            var destProperties = destinationType.GetProperties();
            var srcProperties = sourceType.GetProperties();
            foreach (var prop in destProperties)
            {
                if (srcProperties.Any(p => p.Name == prop.Name))
                {
                    code += string.Format("{0}t2.{1}=({2})t1.{1};\r\n", indent, prop.Name, GetTypeFullName(prop.PropertyType));
                }
            }

            var destFields = destinationType.GetFields();
            var srcFields = sourceType.GetFields();

            foreach (var field in destFields)
            {
                if (srcFields.Any(f => f.Name == field.Name))
                {
                    code += string.Format("{0}t2.{1}=({2})t1.{1};\r\n", indent, field.Name, GetTypeFullName(field.FieldType));
                }
            }

            return code;
        }

        /// <summary>
        /// replace + with . for inner types
        /// </summary>
        /// <param name="type">Type Object</param>
        /// <returns></returns>
        private string GetTypeFullName(Type type)
        {
            return type.FullName.Replace('+','.');
        }

        protected Assembly BuildAssembly(string code, IEnumerable<ITypeMap> typeMaps)
        {
            Dictionary<string, string> paths = new Dictionary<string, string>();
            foreach (var typeMap in typeMaps)
            {
                if (!paths.ContainsKey(typeMap.SourceType.Assembly.Location))
                {
                    paths.Add(typeMap.SourceType.Assembly.Location, typeMap.SourceType.Assembly.Location);
                }

                if (!paths.ContainsKey(typeMap.DestinationType.Assembly.Location))
                {
                    paths.Add(typeMap.SourceType.Assembly.Location, typeMap.DestinationType.Assembly.Location);
                }
            }

            var tree = SyntaxFactory.ParseSyntaxTree(code);
            MetadataReference[] refs = new MetadataReference[paths.Count + 3];
            refs[0] = new MetadataFileReference(typeof(object).Assembly.Location);
            refs[1] = new MetadataFileReference(this.GetType().Assembly.Location);
            refs[2] = new MetadataFileReference(typeof(Enumerable).Assembly.Location);
            int i = 3;
            foreach (var path in paths)
            {
                refs[i] = new MetadataFileReference(path.Value);
                i++;
            }

            var compilation = CSharpCompilation.Create(
                null,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary),
                syntaxTrees: new[] { tree },
                references: refs);

            Assembly compiledAssembly = null;
            using (var stream = new MemoryStream())
            {
                var compileResult = compilation.Emit(stream);
                if (compileResult.Success)
                {
                    compiledAssembly = Assembly.Load(stream.GetBuffer());
                }
            }

            return compiledAssembly;
        }
    }
}