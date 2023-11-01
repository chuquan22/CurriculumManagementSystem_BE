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

        public bool CheckUserDuplicate(string email)
        {
            return userDAO.CheckUserDuplicate(email);
        }

        public string CreateUser(User user)
        {
            return userDAO.CreateUser(user);
        }

        public string DeleteUser(User user)
        {
            return userDAO.DeleteUser(user);
        }

        public List<User> GetAllUser()
        {
            return userDAO.GetAllUser();
        }

        public int GetTotalUser(string? txtSearch)
        {
            return userDAO.GetTotalUser(txtSearch);
        }

        public User GetUserById(int id)
        {
            return userDAO.GetUserById(id);
        }

        public User Login(string username, string password)
        {
            return userDAO.Login(username, password);
        }

        public List<User> PaginationUser(int page, int limit, string? txtSearch)
        {
            return userDAO.PaginationUser(page, limit, txtSearch);
        }

        public string UpdateUser(User user)
        {
            return userDAO.UpdateUser(user);
        }
    }
}
