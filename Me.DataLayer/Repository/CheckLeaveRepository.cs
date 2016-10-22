using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Me.DataLayer.Util;
using Me.Models.Tables;

namespace Me.DataLayer.Repository
{
    public class CheckLeaveRepository
    {
        private readonly DapperContext _dbContext = new DapperContext();

        public async Task<IEnumerable<EmployeeLeave>> EmployeeLeavePerYear(int? employeeId, int? leaveId)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("@EmployeeId", employeeId);
            p.Add("LeaveId", leaveId);

            return
                await
                    _dbContext.Connection.QueryAsync<EmployeeLeave, Leave, Employee, EmployeeLeave>(
                        "CheckLeaveIdPerYear",
                        (employeeLeave, leave, employee) =>
                        {
                            employeeLeave.Leave = leave;
                            employeeLeave.Employee = employee;
                            return employeeLeave;
                        }, p, commandType: CommandType.StoredProcedure,
                        splitOn: "LeaveId, EmployeeId");

        }

        public async Task<IEnumerable<EmployeeLeave>> EmployeePerYear(int? employeeId)
        {
            DynamicParameters p = new DynamicParameters();

            p.Add("@EmployeeId", employeeId);

            return
                await
                    _dbContext.Connection.QueryAsync<EmployeeLeave, Leave, Employee, EmployeeLeave>(
                        "CheckEmployeeIdPerYear",
                        (employeeLeave, leave, employee) =>
                        {
                            employeeLeave.Leave = leave;
                            employeeLeave.Employee = employee;
                            return employeeLeave;
                        }, p, commandType: CommandType.StoredProcedure, splitOn: "LeaveId, EmployeeId");
        }

        public async Task<IEnumerable<EmployeeLeave>> LeavePerYear(int? leaveId)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("@LeaveId", leaveId);

            return
                await
                    _dbContext.Connection.QueryAsync<EmployeeLeave, Leave, Employee, EmployeeLeave>(
                        "CheckLeaveIdPerYear",
                        (employeeLeave, leave, employee) =>
                        {
                            employeeLeave.Leave = leave;
                            employeeLeave.Employee = employee;
                            return employeeLeave;
                        }, p, commandType: CommandType.StoredProcedure, splitOn: "LeaveId, EmployeeId");
        }

        public async Task<IEnumerable<EmployeeLeave>> LeaveInclusiveDate(int? leaveId, DateTime? dateFrom,
            DateTime? dateTo)
        {
            DynamicParameters p = new DynamicParameters();

            p.Add("@LeaveId", leaveId);
            p.Add("@DateFrom", dateFrom);
            p.Add("@DateTo", dateTo);

            return
                await
                    _dbContext.Connection.QueryAsync<EmployeeLeave, Leave, Employee, EmployeeLeave>(
                        "CheckLeaveIdInclusiveDate",
                        (employeeLeave, leave, employee) =>
                        {
                            employeeLeave.Leave = leave;
                            employeeLeave.Employee = employee;
                            return employeeLeave;
                        }, p, commandType: CommandType.StoredProcedure, splitOn: "LeaveId, EmployeeId");
        }

        public async Task<IEnumerable<EmployeeLeave>> LeaveEmployeeInclusiveDate(int? leaveId, int? employeeId,
            DateTime dateFrom, DateTime dateTo)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("@LeaveId", leaveId);
            p.Add("@EmployeeId", employeeId);
            p.Add("@DateFrom", dateFrom);
            p.Add("@DateTo", dateTo);

            return
                await
                    _dbContext.Connection.QueryAsync<EmployeeLeave, Leave, Employee, EmployeeLeave>(
                        "CheckLeaveIdEmployeeIdInclusiveDate",
                        (employeeLeave, leave, employee) =>
                        {
                            employeeLeave.Leave = leave;
                            employeeLeave.Employee = employee;
                            return employeeLeave;
                        }, p, commandType: CommandType.StoredProcedure, splitOn: "LeaveId, EmployeeId");
        }

        public async Task<IEnumerable<EmployeeLeave>> EmployeeInclusiveDate(int? employeeId, DateTime? dateFrom,
            DateTime? dateTo)
        {
            DynamicParameters p = new DynamicParameters();

            p.Add("@EmployeeId", employeeId);
            p.Add("@DateFrom", dateFrom);
            p.Add("@DateTo", dateTo);

            return
                await
                    _dbContext.Connection.QueryAsync<EmployeeLeave, Leave, Employee, EmployeeLeave>(
                        "CheckEmployeeIdInclusiveDate",
                        (employeeLeave, leave, employee) =>
                        {
                            employeeLeave.Leave = leave;
                            employeeLeave.Employee = employee;
                            return employeeLeave;
                        }, p, commandType: CommandType.StoredProcedure, splitOn: "LeaveId, EmployeeId");
        }
    }
}
