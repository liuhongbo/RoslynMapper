using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using RoslynMapper.Map;

namespace RoslynMapper.Map
{
    public class Mapping<T1, T2> : IMapping<T1, T2>
    {
        private ITypeMap<T1,T2> _typeMap = null;

        public Mapping(ITypeMap<T1,T2> typeMap)
        {
            _typeMap = typeMap;            
        }

        public IMapping<T1, T2> For(Expression<Func<T2, object>> t2, Action<IMemberMapping<T1,T2>> mapping)
        {
            IMember<T1,T2> member = GetMember(t2);
            var memberMapping = new MemberMapping<T1,T2>(member);
            mapping(memberMapping);
            return this;
        }

        public IMapping<T1, T2> For(Expression<Func<T1, object>> t1, Action<IMemberMapping<T1, T2>> mapping)
        {
            IMember<T1, T2> member = GetMember(t1);
            var memberMapping = new MemberMapping<T1,T2>(member);
            mapping(memberMapping);
            return this;
        }

        public IMapping<T1, T2> Ignore(Expression<Func<T2, object>> t2)
        {
            return For(t2, m => m.Ignore());
        }

        public IMapping<T1, T2> Ignore(Expression<Func<T1, object>> t1)
        {
            return For(t1, m => m.Ignore());
        }

        public IMapping<T1, T2> Bind(Expression<Func<T1, object>> t1, Expression<Func<T2, object>> t2)
        {
            IMember<T1, T2> member1 = GetMember(t1);
            IMember<T1, T2> member2 = GetMember(t2);
            return For(t2, m => m.Bind(t1));
        }

        public IMapping<T1, T2> Resolve(Expression<Func<T2, object>> t2, Action<T1, T2> resolver)
        {
            return For(t2, m => m.Resolve(resolver));
        }

        public IMapping<T1, T2> Resolve(Action<T1, T2> resolver)
        {
            _typeMap.Resolver = resolver;
            return this;
        }        

        public IMapping<T1, T2> CodeResolve(Func<IMember, string> resolver)
        {
            _typeMap.CodeResolver = resolver;
            return this;
        }

        /// <summary>
        /// Get source member, if not exist, add it
        /// </summary>
        /// <param name="t1"></param>
        /// <returns></returns>
        private IMember<T1, T2> GetMember(Expression<Func<T1, object>> t1)
        {
            Member<T1, T2> m = Member<T1,T2>.FromLambdaExpression(t1);
            var member = _typeMap.Members.GetMember<T1,T2>(m.Key);
            if (member == null)
            {
                _typeMap.Members.AddMember(m);
                member = m;
            }
            return member;
        }

        /// <summary>
        /// Get Destination member, if not exist, add it
        /// </summary>
        /// <param name="t2"></param>
        /// <returns></returns>
        private IMember<T1, T2> GetMember(Expression<Func<T2, object>> t2)
        {
            Member<T1, T2> m = Member<T1, T2>.FromLambdaExpression(t2);
            var member = _typeMap.Members.GetMember<T1,T2>(m.Key);
            if (member == null)
            {
                _typeMap.Members.AddMember(m);
                member = m;
            }
            return member;
        }
    }
}
