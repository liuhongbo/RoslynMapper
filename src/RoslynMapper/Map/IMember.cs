﻿using System;
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
        MemberPath Path { get;}
        string Id { get; }
        bool Ignored { get; set;}
        string FullName { get; }
    }

    public interface IMember<T1, T2> : IMember
    {
        IMember<T1,T2> BindMember { get; set; }
        Action<T1, T2> Resolver { get; set; }
        Func<string> CodeResolver { get; set; }
    }
}
