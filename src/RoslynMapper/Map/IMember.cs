using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RoslynMapper.Map
{
    public interface IMember
    {
        MemberKey Key { get; }
        MemberInfo MemberInfo { get;}
        MemberPath Path { get; set; }        
        bool Ignored { get; set;}        
    }

    public interface IMember<T1, T2> : IMember
    {
        IMember<T1,T2> BindMember { get; set; }
        Action<T1, T2> Resolver { get; set; }
    }
}
