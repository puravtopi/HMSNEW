using Dapper;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;

namespace HMS.Interface
{
    public interface IDapperService
    {

        DbConnection GetDbconnection(string connectionName);
        T Get<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure, string connectionName = "");
        List<T> GetAll<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure, string connectionName = "");
        T Insert<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure, string connectionName = "");
        T Update<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure, string connectionName = "");

    }
}
