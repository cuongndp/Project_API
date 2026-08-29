
using System.Data;
using Dapper;
using System.Linq;
using DataAccess.netCore.Dapper;
using Microsoft.Data.SqlClient;
namespace DataAccess.Dapper
{
    public class ApplicationDbConnection : IApplicationDbConnection,IDisposable
    {
        private readonly IDbConnection connection;
        public ApplicationDbConnection(IConfiguration configuration)
        {
            connection = new SqlConnection(configuration.GetConnectionString("connecing"));
        }
        public IDbConnection GetConnection => connection;

        public void Dispose()
        {
            connection.Dispose();
        }

        // hàm này thêm sửa xóa
        public async Task<int> ExcuteAsync(string sql, object param = null, CommandType commandType = CommandType.StoredProcedure, IDbTransaction transaction = null, CancellationToken cancellationToken = default)
        {
            return (await connection.ExecuteAsync(sql, param, transaction, commandTimeout: 600, commandType: commandType));
        }
        //trả về 1 danh sách
        public async Task<List<T>> QueryAsync<T>(string sql, object param = null, CommandType commandType = CommandType.StoredProcedure, IDbTransaction transaction = null, CancellationToken cancellationToken = default)
        {
            return (await connection.QueryAsync<T>(sql, param, transaction, commandTimeout: 600, commandType: commandType))?.ToList();
        }
        //hàm này trả ra dòng đầu tiên
        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null, CommandType commandType = CommandType.StoredProcedure, IDbTransaction transaction = null, CancellationToken cancellationToken = default)
        {
            // Gọi hàm sinh sẵn của Dapper, nó sẽ tự trả về Object đầu tiên tìm thấy hoặc null
            return (await connection.QueryFirstOrDefaultAsync<T>(sql, param, transaction, commandTimeout: 600, commandType: commandType));
        }
        // hàm này bắt buộc trả ra 1 dòng nếu ko có dòng nào hoặc nhìu hơn 1 nó sẽ lỗi trả ra ex
        public async Task<T> QuerySingleAsync<T>(string sql, object param = null, CommandType commandType = CommandType.StoredProcedure, IDbTransaction transaction = null, CancellationToken cancellationToken = default)
        {
            // Gọi hàm sinh sẵn của Dapper, dùng khi bạn CHẮC CHẮN kết quả chỉ trả về duy nhất 1 dòng (ví dụ theo Id)
            // Nếu DB trả về 0 dòng hoặc nhiều hơn 1 dòng, hàm này sẽ báo lỗi (Exception) ngay lập tức
            return (await connection.QuerySingleAsync<T>(sql, param, transaction, commandTimeout: 600, commandType: commandType));
        }
        public async Task<SqlMapper.GridReader> QueryMultipleAsync(string sql,object param = null,CommandType commandType = CommandType.StoredProcedure,IDbTransaction transaction = null,CancellationToken cancellationToken = default)
        {
            return await connection.QueryMultipleAsync(
                sql,
                param,
                transaction,
                commandTimeout: 600,
                commandType: commandType
            );
        }
    }
}
