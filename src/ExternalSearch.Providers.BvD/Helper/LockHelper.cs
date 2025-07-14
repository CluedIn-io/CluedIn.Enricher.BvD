using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using CluedIn.Core.DataStore;
using CluedIn.Core;
using Microsoft.Data.SqlClient;

namespace CluedIn.ExternalSearch.Providers.BvD.Helper;

public static class LockHelper
{
    public interface ILock : IDisposable
    {

    }

    private class Lock : ILock
    {
        private readonly SqlConnection _connection;

        public Lock(SqlConnection connection)
        {
            _connection = connection;
        }

        public void Dispose()
        {
            _connection.Close();
        }
    }

    public static async Task<ILock> GetDistributedLockAsync(ApplicationContext appContext, string lockName, TimeSpan timeout, CancellationToken cancellationToken = default)
    {
        var connectionStringKey = appContext.System.DataShards.GetDataShard(DataShardType.Locking).ReadConnectionString;
        var connectionString = appContext.System.ConnectionStrings.GetConnectionString(connectionStringKey);

        var conn = new SqlConnection(connectionString);
        await conn.OpenAsync(cancellationToken);
        var t = conn.BeginTransaction();

        await using var cmd = new SqlCommand("sp_getapplock", conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Transaction = t;

        cmd.Parameters.Add(new SqlParameter("@Resource", SqlDbType.NVarChar, 255) { Value = lockName });
        cmd.Parameters.Add(new SqlParameter("@LockMode", SqlDbType.NVarChar, 32) { Value = "Exclusive" });
        cmd.Parameters.Add(new SqlParameter("@LockTimeout", SqlDbType.Int) { Value = timeout.TotalMilliseconds });
        cmd.Parameters.Add(new SqlParameter("@Result", SqlDbType.Int) { Direction = ParameterDirection.ReturnValue });

        await cmd.ExecuteNonQueryAsync(cancellationToken);

        var queryResult = (int)cmd.Parameters["@Result"].Value;

        if (queryResult < 0)
            throw new TimeoutException("AppLockHelper Timeout.");

        return new Lock(conn);
    }
}
