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
            
            Assert.NotEqual(m1 , m2);
        }

        [Fact]
        public void Member_WithSameType_and_SameName_Declared_in_Same_Type_MemberInfo_Always_Equal_1()
        {
            Expression<Func<A, object>> exp1 = p => p.c.Name;
            Expression<Func<A, object>> exp2 = p => p.d.Name;

            var member1 = Member<A,A>.FromLambdaExpression(exp1);
            var member2 = Member<A,A>.FromLambdaExpression(exp2);

            Assert.Equal(member1.MemberInfo , member2.MemberInfo);
        }

        [Fact]
        public void Member_WithSameType_and_SameName_Declared_in_Same_Type_MemberInfo_Always_Equal_2()
        {
            Expression<Func<A, object>> exp1 = p => p.c.Name;
            Expression<Func<B, object>> exp2 = p => p.d.Name;

            var member1 = Member<A,B>.FromLambdaExpression(exp1);
            var member2 = Member<A, B>.FromLambdaExpression(exp2);

            Assert.Equal(member1.MemberInfo, member2.MemberInfo);
        }

        [Fact]
        public void Member_WithSameType_and_SameName_Declared_in_Same_Type_Member_Not_Equal_1()
        {
            Expression<Func<A, object>> exp1 = p => p.c.Name;
            Expression<Func<A, object>> exp2 = p => p.d.Name;

            var member1 = Member<A, A>.FromLambdaExpression(exp1);
            var member2 = Member<A, A>.FromLambdaExpression(exp2);

            Assert.NotEqual(member1, member2);
        }

        [Fact]
        public void Member_WithSameType_and_SameName_Declared_in_Different_Type_Member_Not_Equal_2()
        {
            Expression<Func<A, object>> exp1 = p => p.c.Name;
            Expression<Func<B, object>> exp2 = p => p.c.Name;

            var member1 = Member<A, B>.FromLambdaExpression(exp1);
            var member2 = Member<A, B>.FromLambdaExpression(exp2);

            Assert.NotEqual(member1, member2);
        }

        [Fact]
        public void Member_WithSameType_and_SameName_Declared_in_Different_Type_Member_Not_Equal_3()
        {
            Expression<Func<A, object>> exp1 = p => p.c.Name;
            Expression<Func<B, object>> exp2 = p => p.d.Name;

            var member1 = Member<A, B>.FromLambdaExpression(exp1);
            var member2 = Member<A, B>.FromLambdaExpression(exp2);

            Assert.NotEqual(member1,member2);
        }
    }

    public class MemberPathTests
    {
        public class A
        {
            public int value { get; set; }
            public C c;
            public D d;
        }

        public class B
        {
            public int value;
            public C c;
            public D d;
        }

        public class C
        {
            public int value;
            public D d;
        }

        public class D
        {
            public string value { get; set; }
            public E e { get; set; }
        }

        public class E
        {
            public double value { get; set; }
        }

        [Fact]
        public void Member_Value_From_Type_A()
        {
            Expression<Func<A, object>> exp = p => p.value;

            var m = Member<A, B>.FromLambdaExpression(exp);

            Assert.Equal(m.Path,new MemberPath(typeof(A), string.Empty));
        }

        [Fact]
        public void Member_c_From_Type_A()
        {
            Expression<Func<A, object>> exp = p => p.c;

            var m = Member<A, B>.FromLambdaExpression(exp);

            Assert.Equal(m.Path, new MemberPath(typeof(A), string.Empty));
        }

        [Fact]
        public void Member_Value_From_Type_C()
        {
            Expression<Func<C, object>> exp = p => p.value;

            var m = Member<A, B>.FromLambdaExpression(exp);

            Assert.Equal(m.Path, new MemberPath(typeof(C), string.Empty));
        }

        [Fact]
        public void Member_Value_From_Type_C_in_Type_A()
        {
            Expression<Func<A, object>> exp = p => p.c.value;

            var m = Member<A, B>.FromLambdaExpression(exp);

            Assert.Equal(m.Path, new MemberPath(typeof(A), "c"));
        }

        [Fact]
        public void Member_Value_From_Type_C_in_Type_B()
        {
            Expression<Func<B, object>> exp = p => p.c.value;

            var m = Member<A, B>.FromLambdaExpression(exp);

            Assert.Equal(m.Path, new MemberPath(typeof(B), "c"));
        }

        [Fact]
        public void Member_Value_From_Type_D_in_Type_C_in_Type_A()
        {
            Expression<Func<A, object>> exp = p => p.c.d.value;

            var m = Member<A, B>.FromLambdaExpression(exp);

            Assert.Equal(m.Path, new MemberPath(typeof(A), "c.d"));
        }

        [Fact]
        public void Member_Value_From_Type_D_in_Type_C_in_Type_B()
        {
            Expression<Func<B, object>> exp = p => p.c.d.value;

            var m = Member<A, B>.FromLambdaExpression(exp);

            Assert.Equal(m.Path, new MemberPath(typeof(B), "c.d"));
        }

        [Fact]
        public void Member_value_From_Type_E_in_Type_D_in_Type_C_in_Type_B()
        {
            Expression<Func<B, object>> exp = p => p.c.d.e.value;

            var m = Member<A, B>.FromLambdaExpression(exp);

            Assert.Equal(m.Path, new MemberPath(typeof(B), "c.d.e"));
        }

    }
}
