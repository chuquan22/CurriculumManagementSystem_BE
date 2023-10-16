using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class CLOsDAO
    {
        private readonly CMSDbContext _cmsDbContext = new CMSDbContext();

        public CLO GetCLOsById(int id)
        {
            var rs = _cmsDbContext.CLO
                .Where(c => c.syllabus_id == id)
                .FirstOrDefault();
            return rs;
        }

        public CLO DeleteCLOs(int id)
        {
            var oldCol = _cmsDbContext.CLO.Where(c=> c.CLO_id == id).FirstOrDefault();  
           
            _cmsDbContext.CLO.Remove(oldCol);
            _cmsDbContext.SaveChanges();
            return oldCol;
        }

        public CLO CreateCLOs(CLO clo)
        {
            _cmsDbContext.CLO.Add(clo);
            _cmsDbContext.SaveChanges();
            return clo;
        }

        public CLO UpdateCOLs(CLO clo)
        {
            var oldCol = _cmsDbContext.CLO.Where(c => c.CLO_id == clo.CLO_id).FirstOrDefault();
            oldCol.syllabus_id = clo.syllabus_id;
            oldCol.CLO_name = clo.CLO_name;
            oldCol.CLO_description = clo.CLO_description;
            _cmsDbContext.CLO.Update(oldCol);
            _cmsDbContext.SaveChanges();
            return clo;
        }
    }
}
