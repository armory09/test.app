using Me.Models.Tables;
using Me.Models.View;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Me.DataLayer.Interface
{
    public interface ILeaveRepository
    {
        Task<int> Insert(Leave model);
        Task<IEnumerable<Leave>> SelectAll();
        Task Update(Leave model);
        Task Delete(int? id);
        Task<Leave> SelectByName(string leaveName);
        Task<LeaveCheck> LeaveCheck(int? employeeId, string leaveName);
    }
}
