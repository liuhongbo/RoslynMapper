using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RoslynMapper.Map
{
    public class Member : IMember
    {
        MemberKey _key = null;
        MemberInfo _memberInfo = null;

        public Member(MemberInfo memberInfo, MemberPath path)
        {
            _memberInfo = memberInfo;
            Path = path;
        }

        public MemberInfo MemberInfo
        {
            get
            {
                return _memberInfo;
            }
        }
        public MemberKey Key
        {
            get
            {
                return _key ?? new MemberKey(this.MemberInfo, Path);
            }
        }

        public bool Ignored { get; set; }
        public IMember MapMember { get; set; }
        public MemberPath Path { get; set; }

        //https://github.com/AutoMapper/AutoMapper/blob/develop/src/AutoMapper/Internal/ReflectionHelper.cs
        public static Member FromLambdaExpression(LambdaExpression expression)
        {
            Expression expressionToCheck = expression;

            bool done = false;
            string path = string.Empty;
            MemberInfo memberInfo = null;
            Type originType = null;
            while (!done)
            {
                switch (expressionToCheck.NodeType)
                {
                    case ExpressionType.Convert:
                        expressionToCheck = ((UnaryExpression)expressionToCheck).Operand;
                        break;
                    case ExpressionType.Lambda:
                        expressionToCheck = ((LambdaExpression)expressionToCheck).Body;
                        break;
                    case ExpressionType.MemberAccess:
                        var memberExpression = ((MemberExpression)expressionToCheck);
                        if (memberInfo == null)
                        {
                            memberInfo = memberExpression.Member;
                        }
                        else
                        {
                            path += (string.IsNullOrEmpty(path) ? "" : ".") + memberExpression.Member.Name;
                        }
                        originType = memberExpression.Member.ReflectedType;
                        expressionToCheck = memberExpression.Expression;
                        break;
                    default:
                        done = true;
                        break;
                }
            }

            if ((memberInfo != null) && (originType != null))
            {                
                return new Member(memberInfo, new MemberPath(originType, path));
            }
            return null;
        }
    }
}
