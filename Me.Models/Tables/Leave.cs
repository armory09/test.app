using Me.Models.Interface;

namespace Me.Models.Tables
{
    public class Leave : ILeave
    {
        public int? LeaveId { get; set; }
        public string LeaveName { get; set; }
        public int MaxValue { get; set; }

        public virtual LeaveSpecification LeaveSpecification { get; set; }
    }
}
