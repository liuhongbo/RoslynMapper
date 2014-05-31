using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using RoslynMapper.Map;
using Xunit;

namespace RoslynMapper.UnitTests.Map
{
    public class MemberKeyTests
    {
        public class A
        {
            public string Name { get; set; }            
            public C c;
            public C d;
        }

        public class B
        {
            public string Name { get; set; }
            public C c;
            public C d;
        }

        public class C
        {
            public string Name { get; set; }
        }

        [Fact]
        public void Member_WithSameType_and_SameName_Declared_in_Different_Type_MemberInfo_Not_Equal()
        {
            MemberInfo m1 = typeof(A).GetMember("c")[0];
            MemberInfo m2 = typeof(B).GetMember("c")[0];
            
            Assert.False(m1 == m2);
        }

        [Fact]
        public void Member_WithSameType_and_SameName_Declared_in_Same_Type_MemberInfo_Always_Equal_1()
        {
            Expression<Func<A, object>> exp1 = p => p.c.Name;
            Expression<Func<A, object>> exp2 = p => p.d.Name;

            Member member1 = Member.FromLambdaExpression(exp1);
            Member member2 = Member.FromLambdaExpression(exp2);

            Assert.True(member1.MemberInfo == member2.MemberInfo);
        }

        [Fact]
        public void Member_WithSameType_and_SameName_Declared_in_Same_Type_MemberInfo_Always_Equal_2()
        {
            Expression<Func<A, object>> exp1 = p => p.c.Name;
            Expression<Func<B, object>> exp2 = p => p.d.Name;

            Member member1 = Member.FromLambdaExpression(exp1);
            Member member2 = Member.FromLambdaExpression(exp2);

            Assert.True(member1.MemberInfo == member2.MemberInfo);
        }

        [Fact]
        public void Member_WithSameType_and_SameName_Declared_in_Same_Type_Member_Not_Equal_1()
        {
            Expression<Func<A, object>> exp1 = p => p.c.Name;
            Expression<Func<A, object>> exp2 = p => p.d.Name;

            Member member1 = Member.FromLambdaExpression(exp1);
            Member member2 = Member.FromLambdaExpression(exp2);

            Assert.False(member1.Equals(member2));
        }

        [Fact]
        public void Member_WithSameType_and_SameName_Declared_in_Different_Type_Member_Not_Equal_2()
        {
            Expression<Func<A, object>> exp1 = p => p.c.Name;
            Expression<Func<B, object>> exp2 = p => p.c.Name;

            Member member1 = Member.FromLambdaExpression(exp1);
            Member member2 = Member.FromLambdaExpression(exp2);

            Assert.False(member1.Equals(member2));
        }

        [Fact]
        public void Member_WithSameType_and_SameName_Declared_in_Different_Type_Member_Not_Equal_3()
        {
            Expression<Func<A, object>> exp1 = p => p.c.Name;
            Expression<Func<B, object>> exp2 = p => p.d.Name;

            Member member1 = Member.FromLambdaExpression(exp1);
            Member member2 = Member.FromLambdaExpression(exp2);

            Assert.False(member1.Equals(member2));
        }
    }
}
