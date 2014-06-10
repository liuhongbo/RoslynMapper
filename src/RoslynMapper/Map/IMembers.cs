using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynMapper.Map
{
    public interface IMembers
    {
        IMember GetMember(MemberKey key);
        IMember<T1, T2> GetMember<T1, T2>(MemberKey key);
        IMember<T1, T2> GetMember<T1, T2>(string id);
        void AddMember(IMember Member);
        void RemoveMember(IMember Member);
        IEnumerable<IMember> GetMembers();
        IEnumerable<IMember<T1, T2>> GetMembers<T1, T2>();
        IEnumerable<IMember<T1,T2>> GetMembers<T1,T2>(Type rootType);
        IEnumerable<IMember<T1, T2>> GetMembers<T1,T2>(Type rootType, MemberPath path);
    }
}
