using Me.Models.Interface;
using System;

namespace Me.Models.Tables
{
    public class EmployeeLeave : IEmployeeLeave
    {
        public int EmployeeLeaveId { get; set; }
        public int? LeaveId { get; set; }
        public int? EmployeeId { get; set; }
        public string Reason { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateFiled { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public decimal LeaveAvail { get; set; }
        public int? UserId { get; set; }
        public int? ApprovedId { get; set; }
        public decimal TotalAvail { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual Leave Leave { get; set; }
    }
}
