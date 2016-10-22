using Me.Models.Interface;

namespace Me.Models.Tables
{
    public class Department : IDepartment
    {
        public int? department_id { get; set; }
        public string department_name { get; set; }
        public bool is_active { get; set; }

    }
}
