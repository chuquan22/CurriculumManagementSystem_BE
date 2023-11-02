using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class RoleDAO
    {
        private readonly CMSDbContext _context = new CMSDbContext();

        public List<Role> GetAllRole()
        {
            return _context.Role.ToList();
        }

        public Role GetRole(int id)
        {
            return _context.Role.Find(id);
        }


    }
}
