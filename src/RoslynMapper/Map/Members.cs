using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynMapper.Map
{
    public class Members : Dictionary<MemberKey, IMember>, IMembers
    {
        public IMember GetMember(MemberKey key)
        {
            IMember member = null;
            this.TryGetValue(key, out member);
            return member;
        }

        public void AddMember(IMember Member)
        {
            this.Add(Member.Key, Member);
        }

        public void RemoveMember(IMember Member)
        {
            this.Remove(Member.Key);
        }

        public IEnumerable<IMember> GetMembers()
        {
            var members = new List<IMember>(this.Count);
            foreach (var m in this)
            {
                members.Add(m.Value);
            }
            return members;
        }
    }
}
