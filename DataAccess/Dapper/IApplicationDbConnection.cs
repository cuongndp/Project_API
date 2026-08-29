using Dapper;
using System.Data;

namespace DataAccess.netCore.Dapper
{
    public interface IApplicationDbConnection
    {
        IDbConnection GetConnection { get; }
        Task<int> ExcuteAsync(string sql, object param = null,
            CommandType commandType = CommandType.StoredProcedure,
            IDbTransaction transaction = null, CancellationToken cancellationToken = default);
        Task<List<T>> QueryAsync<T>(
                 string sql,
                 object param = null,
                 CommandType commandType = CommandType.StoredProcedure,
                 IDbTransaction transaction = null,
                 CancellationToken cancellationToken = default);

        Task<T> QueryFirstOrDefaultAsync<T>(
                 string sql,
                 object param = null,
                 CommandType commandType = CommandType.StoredProcedure,
                  IDbTransaction transaction = null,
                 CancellationToken cancellationToken = default
        );

        Task<T> QuerySingleAsync<T>(
                   string sql,
                   object param = null,
                   CommandType commandType = CommandType.StoredProcedure,
                   IDbTransaction transaction = null,
                   CancellationToken cancellationToken = default
        );
        Task<SqlMapper.GridReader> QueryMultipleAsync(
                     string sql,
                     object param = null,
                     CommandType commandType = CommandType.StoredProcedure,
                     IDbTransaction transaction = null,
                     CancellationToken cancellationToken = default
);
    }
}
