using Me.Models.Tables;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Me.DataLayer.Interface
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> SelectAll();
        Task<Employee> SelectById(int? id);
        Task<IEnumerable<Employee>> SelectByEmployeeName(string employeeName);
    }
}
