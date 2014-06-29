using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RoslynMapper.Map
{
    public class MemberMapping<T1,T2> : IMemberMapping<T1,T2>
    {
        private IMember<T1,T2> _member;

        public MemberMapping(IMember<T1,T2> member)
        {
            _member = member;
        }

        public void Ignore()
        {
            _member.Ignored = true;
        }

        public void Bind(Expression<Func<T1, object>> t1)
        {
            var bindMember = Member<T1,T2>.FromLambdaExpression(t1);
            _member.BindMember = bindMember;
        }

        public void Resolve(Action<T1, T2> resolver)
        {
            _member.Resolver = resolver;
        }

        public void CodeResolve(Func<string> resolver)
        {
            _member.CodeResolver = resolver;
        }
    }
}
