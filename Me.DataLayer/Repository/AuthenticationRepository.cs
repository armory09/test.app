using Dapper;
using Me.DataLayer.Interface;
using Me.DataLayer.Util;
using Me.Models.Tables;
using Me.Models.View;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Me.DataLayer.Repository
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly DapperContext _dbContext;

        public AuthenticationRepository()
        {
            _dbContext =  new DapperContext();
        }

        public async Task<AuthenticationView> UserLogin(string username, string password)
        {
            var p = new DynamicParameters();
            bool isAuthenticated = false;


            p.Add("@Username", username);
            p.Add("@Password", password);

            var user = await _dbContext.Connection.QueryAsync<User>("UserLogin", p,
                commandType: CommandType.StoredProcedure);

            try
            {
                var first = user.First();

                if (first.userfile_id != 0)
                {
                    isAuthenticated = true;
                }

                AuthenticationView model = new AuthenticationView
                {
                    IsTrue = isAuthenticated,
                    UserId = first.userfile_id
                };
                return model;
            }
            catch (Exception)
            {
                return null;
            }

        }
    }
}
