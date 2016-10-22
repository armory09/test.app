using Dapper;
using Me.DataLayer.Interface;
using Me.DataLayer.Util;
using Me.Models.Tables;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Me.DataLayer.Repository
{
    public class EmployeeLeaveRepository : IEmployeeLeaveRepository
    {
        private readonly DapperContext _dbContext = new DapperContext();

        public async Task<IEnumerable<Employee>> SelectAll()
        {
            return await _dbContext.Connection.QueryAsync<Employee, Section, Department, EmployeeLeave, Leave, Employee>("EmployeeLeaveSelectAll",
                (employee, section, department, empLeave, leave) =>
                {
                    employee.Department = department;
                    employee.Leave = leave;
                    employee.EmployeeLeave = empLeave;
                    employee.Section = section;
                    return employee;
                }, commandType: CommandType.StoredProcedure,
                splitOn: "section_id, department_id, EmployeeLeaveId, LeaveId");
        }

        public async Task<int?> Insert(EmployeeLeave model)
        {
            var p = new DynamicParameters();

            p.Add("@LeaveId", model.LeaveId);
            p.Add("@EmployeeId", model.EmployeeId);
            p.Add("@Reason", model.Reason);
            p.Add("@DateFiled", model.DateFiled);
            p.Add("@DateFrom", model.DateFrom);
            p.Add("@DateTo", model.DateTo);
            p.Add("@LeaveAvail", model.LeaveAvail);
            p.Add("@UserId", model.UserId);
            p.Add("@ApprovedId", model.ApprovedId);
            p.Add("@EmployeeLeaveId", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await _dbContext.Connection.ExecuteAsync("EmployeeLeaveInsert", p,
                commandType: CommandType.StoredProcedure);

            return p.Get<int>("@EmployeeLeaveId");

        }
    }
}
