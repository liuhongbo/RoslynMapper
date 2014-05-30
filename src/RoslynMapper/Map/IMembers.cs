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
        void AddMember(IMember Member);
        void RemoveMember(IMember Member);
        IEnumerable<IMember> GetMembers();
    }
}
