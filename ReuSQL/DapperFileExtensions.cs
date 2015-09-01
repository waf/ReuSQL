using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Dapper;

namespace ReuSQL
{
    public static class DapperFileExtensions
    {
        public static IEnumerable<TReturn> QueryFromFile<TReturn>(this IDbConnection cnn, string file, object param = null)
        {
            //TODO: cache the file read
            var sql = System.IO.File.ReadAllText(file + ".sql");
            return cnn.Query<TReturn>(sql, param);
        }
    }
}
