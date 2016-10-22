namespace Me.Models.Interface
{
    public interface IDepartment
    {
        int? department_id { get; set; }
        string department_name { get; set; }
        bool is_active { get; set; }
    }
}
