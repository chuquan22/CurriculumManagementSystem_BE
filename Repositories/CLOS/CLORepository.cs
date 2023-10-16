using BusinessObject;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.CLOS
{
    public class CLORepository : ICLORepository
    {
        public readonly CLOsDAO db = new CLOsDAO();

        public CLO CreateCLOs(CLO clo)
        {
            return db.CreateCLOs(clo);
        }

        public CLO DeleteCLOs(int id)
        {
            return db.DeleteCLOs(id);
        }

        public CLO GetCLOsById(int id)
        {
            return db.GetCLOsById(id);
        }

        public CLO UpdateCLOs(CLO clo)
        {
            return db.UpdateCOLs(clo);
        }
    }
}
