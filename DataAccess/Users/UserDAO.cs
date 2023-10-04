using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccess.Models.DTO.request;
using DataAccess.Models.DTO.response;

namespace DataAccess.Users
{
    public class UserDAO
    {
        public CMSDbContext db = new CMSDbContext();
        public User Login(string username, string password)
        {
            UserLoginRequest request = new UserLoginRequest();
            request.username = username;
            request.password = password;
            User response = new User();
            try
            {
                response = db.User.Where(u => u.user_name.Equals(username) && u.user_password.Equals(password)).FirstOrDefault();
            }
            catch (Exception)
            {

                throw;
            }
            return response;
        }
    }
}
