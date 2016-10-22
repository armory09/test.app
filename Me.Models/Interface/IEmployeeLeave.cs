using Me.Models.Tables;
using System;
using System.Collections.Generic;

namespace Me.Models.Interface
{
    public interface IEmployeeLeave
    {
        int EmployeeLeaveId { get; set; }
        int? LeaveId { get; set; }
        int? EmployeeId { get; set; }
        string Reason { get; set; }
        DateTime? DateCreated { get; set; }
        DateTime? DateFiled { get; set; }
        DateTime? DateFrom { get; set; }
        DateTime? DateTo { get; set; }
        decimal LeaveAvail { get; set; }
        int? UserId { get; set; }
        int? ApprovedId { get; set; }
        decimal TotalAvail { get; set; }
        Employee Employee { get; set; }
        Leave Leave { get; set; }
    }
}
