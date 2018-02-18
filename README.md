RoslynMapper
============

Roslyn Mapper is a free open source .NET library. It is an object-object mapper built on top of the [.NET Compiler Platform ("Roslyn")](http://roslyn.codeplex.com/, 'roslyn'). The main goal is to build an object-object mapper with good mapping performance and a convenient API.

### Install

Download from Nuget. Get the latest [RoslynMapper Library Package](http://www.nuget.org/packages/RoslynMapper/, roslynmapper package) from Nuget. Install from the NuGet package manager console:

```
	Install-Package RoslynMapper -Version 0.3.0
```

### Getting started

**Define the source type and destination type**

```csharp
public class A
{
    public string Name { get; set; }
}
public class B
{
    public string Name;
}
```

**Set up the mapper(s) and build it in your program startup**

```csharp
var mapper = RoslynMapper.MapEngine.DefaultInstance;
mapper.SetMapper<A, B>();
mapper.Build();
```

**Map the object from A to B**

```csharp
A a = new A() { Name = "Hello World" };
var b = mapper.Map<a, b>(a);
Console.WriteLine(b.Name);
```
### How it works

##### Using the Microsoft Compile Platform (Rosyln) to compile the mapping logic into a in-memory danymic link library, RoslynMapper is able to achieve a hand write mapping performance.

RoslynMapper does one thing and only does one thing: Map object from one type to another type.

**Define the source type and destination type**

Simple source type and destination type defination.

```csharp
public class A
{
    public string Name { get; set; }
}
public class B
{
    public string Name;
}
```

**Set up the mapper in your program startup**

RoslynMapper will save the configuration for each type map which will be used to create the mapper.

```csharp
var mapper = RoslynMapper.MapEngine.DefaultInstance;
mapper.SetMapper<a, b>();
```

**Build the mapper**

RoslynMapper will generate the source code for the mapper first, the source code looks like this:

```csharp
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoslynMapper.Map;
namespace RoslynMapper.Mapper
{
    class RoslynMapper_Samples_HelloWorld_A__Map__RoslynMapper_Samples_HelloWorld_B__:MapperBase<roslynmapper.samples.helloworld.a,roslynmapper.samples.helloworld.b>,IMapper<roslynmapper.samples.hello world.a,roslynmapper.samples.helloworld.b>
    {
        private IMapEngine _engine;
        private ITypeMap _typeMap;
        public RoslynMapper_Samples_HelloWorld_A__Map__RoslynMapper_Samples_HelloWorld_B__(IMapEngine engine, ITypeMap typeMap)
        {
            _engine = engine;
            _typeMap = typeMap;
        }
        public RoslynMapper.Samples.HelloWorld.B Map(RoslynMapper.Samples.HelloWorld.A t1)
        {
            return Map(t1, new RoslynMapper.Samples.HelloWorld.B());
        }
        public RoslynMapper.Samples.HelloWorld.B Map(RoslynMapper.Samples.HelloWorld.A t1, RoslynMapper.Samples.HelloWorld.B t2)
        {
            t2.Name=t1.Name;
            return t2;
        }
        public override int GetHashCode()
        {
            return 694497719;
        }
        public override IMapEngine MapEngine
        {
            get
            {
                return  _engine;
            }
        }
        public override ITypeMap<roslynmapper.samples.helloworld.a,roslynmapper.samples.helloworld.b> TypeMap
        {
            get
            {
                return (ITypeMap<roslynmapper.samples.helloworld.a,roslynmapper.samples.helloworld.b>)_typeMap;
            }
        }
    }
}
```

**Use Microsoft Compile Platform to compile the source code to a in-memory DLL**

The code that does the compiling looks like this:

```csharp
protected Assembly BuildAssembly(string code, IEnumerable<itypemap> typeMaps, string assemblyName)
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
```

**Create the mapper instance that support the IMapper interface**

RoslynMapper will create all the mappers right after the compiling using reflection.

```csharp
Type mapType = assembly.GetType(string.Format("{0}.{1}", GetNamespace(typeMaps), typeMap.MapperName));
if (mapType != null)
{
    IMapper mapper = (IMapper)Activator.CreateInstance(mapType, engine, typeMap);
    if (mapper != null)
    {
        mappers.Add(new KeyValuePair<mapkey, imapper>(typeMap.Key, mapper));
    }
}   
```

**Use the IMapper interface to map the object from one type to another**

RoslynMapper will return the corresponsive mapper for the specific type map which support the IMapper interfae.

```csharp
A a = new A() { Name = "Hello World" };
var b = mapper.Map<a, b>(a);
```


### Document

For more document, please visit [wiki](https://github.com/liuhongbo/RoslynMapper/wiki).

