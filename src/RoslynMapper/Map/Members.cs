﻿using System;
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

        public IMember<T1, T2> GetMember<T1, T2>(MemberKey key)
        {
            return (IMember<T1, T2>)(GetMember(key));
        }

        public IMember<T1, T2> GetMember<T1, T2>(string id)
        {
            return (IMember<T1, T2>)this.FirstOrDefault(m => m.Value.Id == id).Value;
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

        public IEnumerable<IMember<T1, T2>> GetMembers<T1, T2>()
        {
            var members = new List<IMember<T1,T2>>(this.Count);
            foreach (var m in this)
            {
                members.Add((IMember<T1,T2>)m.Value);
            }
            return members; 
        }

        public IEnumerable<IMember<T1, T2>> GetMembers<T1, T2>(Type rootType)
        {
            return GetMembers<T1, T2>().Where(m => m.Path.RootType.Equals(rootType));
        }

        public IEnumerable<IMember<T1, T2>> GetMembers<T1, T2>(Type rootType, MemberPath path)
        {
            return GetMembers<T1, T2>().Where(m => m.Path.Equals(path));
        }
    }
}
