using Dapper;
using System.Data;

namespace Api.Analytics.Infra;
public class DbSession
{
    private readonly IDbConnection _conn;

    public DbSession(IDbConnection conn)
    {
        _conn = conn;
    }

    public async Task<IReadOnlyList<T>> QueryAsync<T>(string sql, object? param = null)
    {
        try
        {
            var result = await _conn.QueryAsync<T>(sql, param);
            return result.AsList();
        }
        finally
        {
            _conn.Close();
        }
    }

    public async Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? param = null)
    {
        try
        {
            return await _conn.QueryFirstOrDefaultAsync<T>(sql, param);
        }
        finally
        {
            _conn.Close();
        }
    }
}
