namespace Me.Models.View
{
    public class LeaveView
    {
        public int? LeaveId { get; set; }
        public string Name { get; set; }
        public decimal MaxValue { get; set; }
        public bool Annual { get; set; }
        public bool Regular { get; set; }
        public bool Service { get; set; }
        public int ServiceLength { get; set; }
    }
}
