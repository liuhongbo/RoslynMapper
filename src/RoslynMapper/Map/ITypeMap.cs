﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynMapper.Map
{
    public interface ITypeMap
    {
        string Name { get; set; }
        int Key { get; }
        Type SourceType { get; }
        Type DestinationType { get; }
    }

    public interface ITypeMap<T1, T2> : ITypeMap
    {

    }
}
