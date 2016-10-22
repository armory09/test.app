using Me.Models.Interface;

namespace Me.Models.Tables
{
    public class Section : ISection
    {
        public int? section_id { get; set; }
        public string section_name { get; set; }
        public int department_id { get; set; }
        public bool is_active { get; set; }
    }
}
