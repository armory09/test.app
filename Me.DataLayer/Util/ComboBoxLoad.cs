using Me.DataLayer.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Me.DataLayer.Util
{
    public class ComboBoxLoad
    {
        private readonly LeaveRepository _leaveRepo = new LeaveRepository();

        public async Task<List<string>> Load()
        {
            var result = await _leaveRepo.SelectAll();
            var leaveList = new List<string>();

            foreach(var item in result)
            {
                leaveList.Add(item.LeaveName);
            }

            return leaveList;
        }
    }
}
