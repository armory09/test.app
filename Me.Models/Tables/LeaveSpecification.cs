using Me.Models.Interface;

namespace Me.Models.Tables
{
    public class LeaveSpecification : ILeaveSpecification
    {
        public int LeaveSpecificationId { get; set; }
        public int LeaveId { get; set; }
        public bool IsAnnual { get; set; }
        public bool IsRegular { get; set; }
        public bool IsYearOfService { get; set; }
        public int ServiceLength { get; set; }
    }
}
