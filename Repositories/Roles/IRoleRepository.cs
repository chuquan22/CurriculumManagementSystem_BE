using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Roles
{
    public interface IRoleRepository
    {
        List<Role> GetAllRole();
        Role GetRoleById(int id);
    }
}
