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

        public Member(MemberInfo memberInfo)
        {
            _memberInfo = memberInfo;
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
                return _key ?? new MemberKey(this.MemberInfo);
            }
        }

        public bool Ignored { get; set; }
        public IMember MapMember { get; set; }

        public static Member FromLambdaExpression(LambdaExpression expression)
        {
            Expression expressionToCheck = expression;

            bool done = false;

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

                        if (memberExpression.Expression.NodeType != ExpressionType.Parameter &&
                            memberExpression.Expression.NodeType != ExpressionType.Convert)
                        {
                            throw new ArgumentException(string.Format("Expression '{0}' must resolve to top-level member and not any child object's properties. Use a custom resolver on the child type or the AfterMap option instead.", expression), "lambdaExpression");
                        }

                        MemberInfo memberInfo = memberExpression.Member;

                        return new Member(memberInfo);
                    default:
                        done = true;
                        break;
                }
            }

            throw new Exception("Custom configuration for members is only supported for top-level individual members on a type.");
        }
    }
}
