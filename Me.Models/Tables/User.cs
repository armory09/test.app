using Me.Models.Interface;

namespace Me.Models.Tables
{
    public class User : IUser
    {
        public int userfile_id { get; set; }
        public int user_rights_id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }
}
