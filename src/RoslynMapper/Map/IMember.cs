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
        IMember MapMember { get; set; }
        bool Ignored { get; set;}        
    }
}
