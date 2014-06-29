using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynMapper.Data
{
    public static class DbConnectionExtensions
    {
        public static T Query<T>(this IDbConnection connection, IMapEngine mapper, string sql)
        {
            throw new Exception("");
        }

        
    }
}
