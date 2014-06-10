using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynMapper.Map
{
    /// <summary>
    /// the path to locate a member in the typemap
    /// </summary>
    public class MemberPath
    {
        public MemberPath(Type rootType, string accessPath)
        {
            RootType = rootType;
            AccessPath = accessPath;
        }

        public Type RootType { get; set; }
        public string AccessPath { get; set; }

        public override bool Equals(object obj)
        {
            var rhs = (MemberPath)obj;
            return (rhs.RootType.Equals(this.RootType) && (rhs.AccessPath == this.AccessPath));
        }

        public override int GetHashCode()
        {
            return (RootType.GetHashCode() * 397 + AccessPath.GetHashCode());
        }

        public static bool operator ==(MemberPath a, MemberPath b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(MemberPath a, MemberPath b)
        {
            return (!a.Equals(b));
        }


        /// <summary>
        /// MemberPath a is a parent of MemberPath b
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator >(MemberPath a, MemberPath b)
        {
            if (!a.RootType.Equals(b.RootType)) return false;

            string[] p1 = a.AccessPath.Split('.');
            string[] p2 = b.AccessPath.Split('.');

            if (p1.Length >= p2.Length) return false;

            for (int i = 0; i < p1.Length; i++)
            {
                if (p1[i] != p2[i]) return false;
            }

            return true;
        }

        /// <summary>
        /// MemberPath a is a child of Memberpath b
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator <(MemberPath a, MemberPath b)
        {
            if (!a.RootType.Equals(b.RootType)) return false;

            string[] p1 = a.AccessPath.Split('.');
            string[] p2 = b.AccessPath.Split('.');

            if (p1.Length <= p2.Length) return false;

            for (int i = 0; i < p2.Length; i++)
            {
                if (p1[i] != p2[i]) return false;
            }

            return true;
        }

        public static bool operator >=(MemberPath a, MemberPath b)
        {
            return (a == b) || (a > b);
        }

        public static bool operator <=(MemberPath a, MemberPath b)
        {
            return (a == b) || (a < b);
        }
    }
}
