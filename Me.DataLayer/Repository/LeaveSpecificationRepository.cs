using Dapper;
using Me.DataLayer.Util;
using Me.Models.Tables;
using System.Data;
using System.Threading.Tasks;

namespace Me.DataLayer.Repository
{
    public class LeaveSpecificationRepository
    {
        private readonly DapperContext _dbContext;

        public LeaveSpecificationRepository()
        {
            _dbContext = new DapperContext();
        }

        public async Task Insert(LeaveSpecification model)
        {
            DynamicParameters p = new DynamicParameters();

            p.Add("@LeaveId", model.LeaveId);
            p.Add("@IsAnnual", model.IsAnnual);
            p.Add("@IsRegular", model.IsRegular);
            p.Add("@IsYearOfService", model.IsYearOfService);
            p.Add("@ServiceLength", model.ServiceLength);

            await _dbContext.Connection.ExecuteAsync("LeaveSpecificationInsert", p, commandType: CommandType.StoredProcedure);
        }
    }
}
