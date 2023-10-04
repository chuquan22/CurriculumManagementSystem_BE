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
        public User Login(string username, string password);
    }
}
