using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;

namespace Web.RepositoryContract
{
    public interface IRepository
    {
        IDbDataParameter CreateParameter(string name, DbType type, object value = null);
        Task<long> ExecuteNonQuery(string sql, List<IDbDataParameter> parameters = null);
    }
}