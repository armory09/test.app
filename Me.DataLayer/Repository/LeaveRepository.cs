using Dapper;
using Me.DataLayer.Interface;
using Me.DataLayer.Util;
using Me.Models.Tables;
using Me.Models.View;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Me.DataLayer.Repository
{
    public class LeaveRepository : ILeaveRepository
    {
        private readonly DapperContext _dbContext = new DapperContext();

        public async Task<int> Insert(Leave model)
        {
            var p = new DynamicParameters();

            p.Add("@LeaveName", model.LeaveName);
            p.Add("@MaxValue", model.MaxValue);
            p.Add("@LeaveId", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await _dbContext.Connection.ExecuteAsync("LeaveInsert", p, commandType: CommandType.StoredProcedure);

            return p.Get<int>("@LeaveId");
        }

        public async Task<IEnumerable<Leave>> SelectAll()
        {
            //return await _dbContext.Connection.QueryAsync<Leave>("LeaveSelectAll", commandType: CommandType.StoredProcedure);

            return await _dbContext.Connection.QueryAsync<Leave, LeaveSpecification, Leave>("LeaveSelectAll",
                (leave, leaveSpecification) =>
                {
                    leave.LeaveSpecification = leaveSpecification;
                    return leave;
                }, commandType: CommandType.StoredProcedure,
                splitOn: "LeaveSpecificationId");

        }

        public async Task Update(Leave model)
        {
            var p = new DynamicParameters();
            p.Add("@LeaveId", model.LeaveId);
            p.Add("@LeaveName", model.LeaveName);
            p.Add("@MaxValue", model.MaxValue);

            await _dbContext.Connection.ExecuteAsync("LeaveUpdate", p, commandType: CommandType.StoredProcedure);
        }

        public async Task Delete(int? id)
        {
            var p = new DynamicParameters();
            p.Add("@LeaveId", id);

            await _dbContext.Connection.ExecuteAsync("LeaveDelete", p,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<Leave> SelectByName(string leaveName)
        {
            var p = new DynamicParameters();
            p.Add("@LeaveName", leaveName);

            var result = await _dbContext.Connection.QueryAsync<Leave>("LeaveSelectByName", p, commandType: CommandType.StoredProcedure);

            return result.FirstOrDefault();
        }

        public async Task<int> LeaveCheckEmployee(int employeeId)
        {
            var p = new DynamicParameters();
            p.Add("@EmployeeId", employeeId);
            p.Add("@NoOfMonth", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await _dbContext.Connection.ExecuteAsync("LeaveCheckEmployee", p, commandType: CommandType.StoredProcedure);

            return p.Get<int>("@NoOfMonth");
        }

        public async Task<LeaveCheck> LeaveCheck(int? employeeId, string leaveName)
        {
            var p = new DynamicParameters();
            p.Add("@EmployeeId", employeeId);
            p.Add("@LeaveName", leaveName);

            var result = await _dbContext.Connection.QueryAsync<LeaveCheck>("LeaveCheckEmployee", p,
                commandType: CommandType.StoredProcedure);

            return result.FirstOrDefault();
        }
    }
}
