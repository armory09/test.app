using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Me.DataLayer.Interface
{
    public interface IDapperContext
    {
        IDbConnection Connection { get; }
    }
}
