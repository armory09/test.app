using Me.Models.Tables;

namespace Me.Models.Interface
{
    public interface ILeave
    {
        int? LeaveId { get; set; }
        string LeaveName { get; set; }
        int MaxValue { get; set; }
        LeaveSpecification LeaveSpecification { get; set; }
    }
}
