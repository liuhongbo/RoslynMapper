using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RoslynMapper.Map
{ 
    public class Member<T1,T2> : IMember<T1,T2>
    {
        private MemberKey _key = null;
        private MemberInfo _memberInfo = null;
        private string _id = null;
        private MemberPath _memberPath = null;

        public Member(MemberInfo memberInfo, MemberPath path)           
        {
            _memberInfo = memberInfo;
            _memberPath = path;
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

        public string Id
        {
            get
            {
                if (_id == null)
                {
                    _id = Guid.NewGuid().ToString();
                }
                return _id;
            }
        }

        public bool Ignored { get; set; }
        public IMember<T1,T2> BindMember { get; set; }
        public MemberPath Path
        {
            get
            {
                return _memberPath;
            }
        }


        /// <summary>
        /// path+name
        /// </summary>
        public string FullName
        {
            get
            {
                return Path.AccessPath + (string.IsNullOrEmpty(Path.AccessPath) ? "" : ".") + MemberInfo.Name;
            }
        }

        public Action<T1, T2> Resolver { get; set; }

        public Func<string> CodeResolver { get; set; }

        //https://github.com/AutoMapper/AutoMapper/blob/develop/src/AutoMapper/Internal/ReflectionHelper.cs
        public static new Member<T1,T2> FromLambdaExpression(LambdaExpression expression)
        {
            Expression expressionToCheck = expression;

            bool done = false;
            string path = string.Empty;
            MemberInfo memberInfo = null;
            Type rootType = null;
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
                            path = memberExpression.Member.Name + (string.IsNullOrEmpty(path) ? "" : ".") + path;
                        }
                        rootType = memberExpression.Member.ReflectedType;
                        expressionToCheck = memberExpression.Expression;
                        break;
                    case ExpressionType.Call:
                        var methodCallExpression = (MethodCallExpression)expressionToCheck;
                        if (memberInfo == null)
                        {
                            memberInfo = methodCallExpression.Method;
                        }
                        else
                        {
                            path = methodCallExpression.Method.Name + (string.IsNullOrEmpty(path) ? "" : ".") + path;
                        }
                        rootType = methodCallExpression.Method.ReflectedType;
                        expressionToCheck = methodCallExpression.Object;
                        break;
                    default:
                        done = true;
                        break;
                }
            }

            if ((memberInfo != null) && (rootType != null))
            {                
                return new Member<T1,T2>(memberInfo, new MemberPath(rootType, path));
            }
            return null;
        }
    }
}
