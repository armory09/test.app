namespace Me.Models.Interface
{
    public interface ISection
    {
        int? section_id { get; set; }
        string section_name { get; set; }
        int department_id { get; set; }
        bool is_active { get; set; }
    }
}
