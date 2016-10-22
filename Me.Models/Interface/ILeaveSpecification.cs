namespace Me.Models.Interface
{
    public interface ILeaveSpecification
    {
        int LeaveSpecificationId { get; set; }
        int LeaveId { get; set; }
        bool IsAnnual { get; set; }
        bool IsRegular { get; set; }
        bool IsYearOfService { get; set; }
        int ServiceLength { get; set; }
    }
}
