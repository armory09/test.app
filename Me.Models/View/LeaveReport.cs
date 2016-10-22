using System;

namespace Me.Models.View
{
    public class LeaveReport
    {
        public string FullName { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string LeaveName { get; set; }
        public decimal Availed { get; set; }
        public decimal Remaining { get; set; }

    }
}