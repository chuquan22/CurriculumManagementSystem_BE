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

        public void DeleteRefreshToken(int user_id)
        {
            userDAO.DeleteRefreshToken(user_id);
        }

        public void DeleteRefreshTokenUser(int user_id)
        {
            userDAO.DeleteRefreshToken(user_id);
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

        public User GetUserByRefreshToken(string refreshToken)
        {
            return userDAO.GetUserByRefreshToken(refreshToken);
        }

        public User Login(string email)
        {
            return userDAO.Login(email);
        }

        public List<User> PaginationUser(int page, int limit, string? txtSearch)
        {
            return userDAO.PaginationUser(page, limit, txtSearch);
        }

        public void SaveRefreshTokenUser(int user_id, string refreshToken)
        {
            userDAO.SaveRefreshTokenUser(user_id, refreshToken);
        }

        public string UpdateUser(User user)
        {
            return userDAO.UpdateUser(user);
        }
    }
}
