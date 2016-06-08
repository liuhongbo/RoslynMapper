using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using RoslynMapper.Map;
using RoslynMapper.Convert;

namespace RoslynMapper.Builder
{
    public class MapperBuilder : IMapperBuilder
    {
        public IEnumerable<KeyValuePair<MapKey, IMapper>> Build(IEnumerable<ITypeMap> typeMaps, IMapEngine engine)
        {
            var mappers = new List<KeyValuePair<MapKey, IMapper>>();

            if (typeMaps.Count() == 0) return mappers;

            var code = GenerateSourceCode(typeMaps);

            Assembly assembly = BuildAssembly(code, typeMaps);

            if (assembly != null)
            {
                foreach (var typeMap in typeMaps)
                {
                    Type mapType = assembly.GetType(string.Format("{0}.{1}", GetNamespace(typeMaps), typeMap.MapperName));
                    if (mapType != null)
                    {
                        IMapper mapper = (IMapper)Activator.CreateInstance(mapType, engine, typeMap);
                        if (mapper != null)
                        {
                            mappers.Add(new KeyValuePair<MapKey, IMapper>(typeMap.Key, mapper));
                        }
                    }
                }
            }

            //Console.WriteLine(sb.ToString());

            return mappers;
        }

        public string GenerateSourceCode(IEnumerable<ITypeMap> typeMaps)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var typeMap in typeMaps)
            {
                sb.Append(typeMap.CreateMapper());
            }

            if (sb.Length > 0)
            {
                sb.Insert(0, string.Format(@"
{0}

namespace {1}
{{", GetUsings(typeMaps), GetNamespace(typeMaps)));

                sb.Append("}");
            }

            return sb.ToString();
        }

        public KeyValuePair<MapKey, IMapper> Build(ITypeMap typeMap, IMapEngine engine)
        {
            IMapper mapper = null;

            var code = GenerateSourceCode(typeMap);

            Assembly assembly = BuildAssembly(code, new ITypeMap[]{ typeMap });

            if (assembly != null)
            {
                Type mapType = assembly.GetType(string.Format("{0}.{1}", GetNamespace(new ITypeMap[] { typeMap }), typeMap.MapperName));
                mapper = (IMapper)Activator.CreateInstance(mapType, engine, typeMap);
            }
            return new KeyValuePair<MapKey, IMapper>(typeMap.Key, mapper);
        }

        public string GenerateSourceCode(ITypeMap typeMap)
        {
            var classCode = typeMap.CreateMapper();

            var code = string.Format(
@"{0}

namespace {1}
{{
    {2}
}}
", GetUsings(new ITypeMap[] { typeMap }), GetNamespace(new ITypeMap[] { typeMap }), classCode);

            return code;
        }

        protected string GetNamespace(IEnumerable<ITypeMap> typeMaps)
        {
            return "RoslynMapper.Mapper";
        }

        protected string GetUsings(IEnumerable<ITypeMap> typeMaps)
        {
            return
@"using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoslynMapper.Map;";            
        }

        

        private IEnumerable<Type> GetParentTypes(Type type)
        {
            // is there any base type?
            if ((type == null) || (type.BaseType == null))
            {
                yield break;
            }

            // return all implemented or inherited interfaces
            foreach (var i in type.GetInterfaces())
            {
                yield return i;
            }

            // return all inherited types
            var currentBaseType = type.BaseType;
            while (currentBaseType != null)
            {
                yield return currentBaseType;
                currentBaseType = currentBaseType.BaseType;
            }
        }


        private static Type[] _defaultReferenceTypes = {typeof(object), typeof(Enumerable), typeof(System.ComponentModel.TypeDescriptor), typeof(MapperBuilder)};

        private IEnumerable<MetadataReference> GetMetadataFileReferences(IEnumerable<ITypeMap> typeMaps)
        {
            Dictionary<string, string> paths = new Dictionary<string, string>();
            foreach (var typeMap in typeMaps)
            {
                if (!paths.ContainsKey(typeMap.SourceType.Assembly.Location))
                {
                    paths.Add(typeMap.SourceType.Assembly.Location, typeMap.SourceType.Assembly.Location);
                }

                foreach (var baseType in GetParentTypes(typeMap.SourceType))
                {
                    if (!paths.ContainsKey(baseType.Assembly.Location))
                    {
                        paths.Add(baseType.Assembly.Location, baseType.Assembly.Location);
                    }
                }

                if (!paths.ContainsKey(typeMap.DestinationType.Assembly.Location))
                {
                    paths.Add(typeMap.DestinationType.Assembly.Location, typeMap.DestinationType.Assembly.Location);
                }

                foreach (var baseType in GetParentTypes(typeMap.DestinationType))
                {
                    if (!paths.ContainsKey(baseType.Assembly.Location))
                    {
                        paths.Add(baseType.Assembly.Location, baseType.Assembly.Location);
                    }
                }
            }

            foreach (var t in _defaultReferenceTypes)
            {
                if (!paths.ContainsKey(t.Assembly.Location))
                {
                    paths.Add(t.Assembly.Location,t.Assembly.Location);
                }
            }

            MetadataReference[] refs = new MetadataReference[paths.Count];

            int i = 0;
           
            foreach (var path in paths)
            {
                refs[i] = MetadataReference.CreateFromFile(path.Value);
                i++;
            }

            return refs;
        }

        protected Assembly BuildAssembly(string code, IEnumerable<ITypeMap> typeMaps)
        {
            return BuildAssembly(code, typeMaps, null);
        }

        protected Assembly BuildAssembly(string code, IEnumerable<ITypeMap> typeMaps, string assemblyName)
        {
            var tree = SyntaxFactory.ParseSyntaxTree(code);            

            var compilation = CSharpCompilation.Create(
                assemblyName,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary),
                syntaxTrees: new[] { tree },
                references: GetMetadataFileReferences(typeMaps));

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
