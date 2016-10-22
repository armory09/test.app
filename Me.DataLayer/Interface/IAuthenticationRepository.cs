using Me.Models.View;
using System.Threading.Tasks;

namespace Me.DataLayer.Interface
{
    public interface IAuthenticationRepository
    {
        Task<AuthenticationView> UserLogin(string username, string password);
    }
}
