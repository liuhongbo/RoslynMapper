using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynMapper.Map
{
    public class MemberMapping : IMemberMapping
    {
        private IMember _member;

        public MemberMapping(IMember member)
        {
            _member = member;
        }

        public void Ignore()
        {
            _member.Ignored = true;
        }
    }
}
