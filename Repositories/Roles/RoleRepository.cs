using BusinessObject;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Roles
{
    public class RoleRepository : IRoleRepository
    {
        private readonly RoleDAO _roleDAO = new RoleDAO();
        public List<Role> GetAllRole()
        {
            return _roleDAO.GetAllRole();
        }

        public Role GetRoleById(int id)
        {
            return _roleDAO.GetRole(id);
        }
    }
}
