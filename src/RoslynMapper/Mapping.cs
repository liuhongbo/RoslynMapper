using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using RoslynMapper.Map;

namespace RoslynMapper
{
    public class Mapping<T1, T2> : IMapping<T1, T2>
    {
        private ITypeMap _typeMap = null;
        public Mapping(ITypeMap typeMap)
        {
            _typeMap = typeMap;
        }

        public IMapping<T1, T2> Ignore(Expression<Func<T2, object>> destinationMember)
        {
            return this;
        }
    }
}
