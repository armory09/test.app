using Me.Models.Tables;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Me.DataLayer.Interface
{
    public interface IEmployeeLeaveRepository
    {
        Task<IEnumerable<Employee>> SelectAll();
        Task<int?> Insert(EmployeeLeave model);
    }
}
