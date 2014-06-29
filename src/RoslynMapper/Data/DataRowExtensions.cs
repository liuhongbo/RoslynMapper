using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynMapper.Data
{
    public static class DataRowExtensions
    {
        public static T Get<T>(this DataRow row, IMapEngine mapper)
        {
            return mapper.Map<DataRow, T>(row);
        }

        public static DataRow Set<T>(this DataRow row, T t)
        {
            return Set<T>(row, MapEngine.DefaultInstance, t);
        }

        public static DataRow Set<T>(this DataRow row, IMapEngine mapper, T t)
        {
            return mapper.Map<T, DataRow>(t);
        }
    }
}
