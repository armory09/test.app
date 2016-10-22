namespace Me.Models.Interface
{
    public interface IUser
    {
        int userfile_id { get; set; }
        int user_rights_id { get; set; }
        string username { get; set; }
        string password { get; set; }
    }
}
