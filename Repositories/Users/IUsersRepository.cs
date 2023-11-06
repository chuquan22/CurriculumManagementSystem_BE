using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace Repositories.Users
{
    public interface IUsersRepository
    {
        public User Login(string email);
        List<User> GetAllUser();
        User GetUserById(int id);
        List<User> PaginationUser(int page, int limit, string? txtSearch);
        int GetTotalUser(string? txtSearch);
        bool CheckUserDuplicate(string email);
        string CreateUser(User user);
        string UpdateUser(User user);
        string DeleteUser(User user);
    }
}
