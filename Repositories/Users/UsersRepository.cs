using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Users;

namespace Repositories.Users
{
    public class UsersRepository : IUsersRepository
    {
        public UserDAO userDAO = new UserDAO();
        
        public User Login(string username, string password)
        {
            return userDAO.Login(username, password);
        }
    }
}
