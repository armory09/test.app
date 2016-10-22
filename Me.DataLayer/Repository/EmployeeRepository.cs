using Me.DataLayer.Interface;
using Me.Models.Tables;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using System.Linq;
using Me.DataLayer.Util;

namespace Me.DataLayer.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DapperContext _dbContext = new DapperContext();

        public EmployeeRepository()
        {
        }

        public async Task<IEnumerable<Employee>> SelectAll()
        {
            var employee = await _dbContext.Connection.QueryAsync<Employee, Department, Section, Employee>("EmployeeSelectAll",
                (emp, department, section) =>
                {
                    emp.Department = department;
                    emp.Section = section;
                    return emp;
                }, commandType: CommandType.StoredProcedure,
                splitOn: "MiddleName, department_name ,section_name");
            return employee;
        }

        public async Task<Employee> SelectById(int? id)
        {
            //var p = new DynamicParameters();
            //p.Add("@EmployeeId", id);

            //var result = await _dbContext.Connection.QueryAsync<Employee>("EmployeeSelectById",
            //    p, commandType: CommandType.StoredProcedure);

            //return result.FirstOrDefault();

            var p = new DynamicParameters();
            p.Add("@EmployeeId", id);

            var result = await _dbContext.Connection.QueryAsync<Employee, Department, Section, Employee>("EmployeeSelectById",
                (emp, department, section) =>
                {
                    emp.Department = department;
                    emp.Section = section;
                    return emp;
                }, p, commandType: CommandType.StoredProcedure,
                splitOn: "MiddleName, department_name ,section_name");
            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<Employee>> SelectByEmployeeName(string employeeName)
        {
            var p = new DynamicParameters();
            p.Add("@EmployeeName", employeeName);

            return await _dbContext.Connection.QueryAsync<Employee>("EmployeeSelectByName",
                p, commandType: CommandType.StoredProcedure);

        }

    }
}
