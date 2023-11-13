using BusinessObject;
using DataAccess.Models.DTO.response;
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

        public List<CLO> GetCLOs(int id)
        {
            var rs = _cmsDbContext.CLO
                .Where(c => c.syllabus_id == id)
                .ToList();
            return rs;
        }

        public CLO DeleteCLOs(int id)
        {
            var oldCol = _cmsDbContext.CLO.Where(c=> c.CLO_id == id).FirstOrDefault();  
           
            _cmsDbContext.CLO.Remove(oldCol);
            _cmsDbContext.SaveChanges();
            return oldCol;
        }

        public CLO GetCLOByName(string name)
        {
            var rs = _cmsDbContext.CLO.Where(c => c.CLO_name.Equals(name)).FirstOrDefault();
            return rs;
        }

        public CLO GetCLOsById(int id)
        {
            var oldCol = _cmsDbContext.CLO.Where(c => c.CLO_id == id).FirstOrDefault();
            _cmsDbContext.SaveChanges();
            return oldCol;
        }
        public bool IsClosExist(string name,int syllabus_id)
        {
            var col = _cmsDbContext.CLO.Where(c => c.CLO_name.ToLower().Trim().Equals(name.ToLower().Trim()) && c.syllabus_id==syllabus_id).FirstOrDefault();
            if(col != null)
            {
                return true;
            }
            return false;
        }
        public CLO CreateCLOs(CLO clo)
        {
            if (!IsValidCLOName(clo.CLO_name))
            {
                throw new Exception("Invalid CLO name. It must start with 'CLO' and be followed by at least one number, and not contain spaces.");
            }

            var rs = _cmsDbContext.CLO
                .Where(c => c.CLO_name.ToLower().Trim().Equals(clo.CLO_name.ToLower().Trim()) && c.syllabus_id == clo.syllabus_id)
                .FirstOrDefault();

            if (rs == null)
            {
                _cmsDbContext.CLO.Add(clo);
                _cmsDbContext.SaveChanges();
                return clo;
            }
            else
            {
                throw new Exception("CLO name already exists in the system.");
            }
        }

        // Validation method for CLO name
        private bool IsValidCLOName(string cloName)
        {
            const string prefix = "CLO";

            // Check if it starts with 'CLO'
            if (!cloName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            // Check if it has at least one number after 'CLO'
            int numberStartIndex = prefix.Length;
            if (numberStartIndex >= cloName.Length || !char.IsDigit(cloName[numberStartIndex]))
            {
                return false;
            }

            // Check if it does not contain spaces 
            if (cloName.Contains(" "))
            {
                return false;
            }

            return true;
        }


        public CLO UpdateCOLs(CLO clo)
        {
            var oldCol = _cmsDbContext.CLO.Where(c => c.CLO_id == clo.CLO_id).FirstOrDefault();
            if (oldCol.CLO_name.ToLower().Trim().Equals(clo.CLO_name.ToLower().Trim()))
            {

            oldCol.syllabus_id = clo.syllabus_id;
            oldCol.CLO_name = oldCol.CLO_name;
            oldCol.CLO_description = clo.CLO_description;
            _cmsDbContext.CLO.Update(oldCol);
            _cmsDbContext.SaveChanges();
            }
            else
            {
                var check = IsClosExist(clo.CLO_name,clo.syllabus_id);
                if (!check)
                {
                    oldCol.syllabus_id = clo.syllabus_id;
                    oldCol.CLO_name = clo.CLO_name;
                    oldCol.CLO_description = clo.CLO_description;
                    _cmsDbContext.CLO.Update(oldCol);
                    _cmsDbContext.SaveChanges();
                }
                else
                {
                    throw new Exception("CLO name already exist in system.");
                }
            }
            return clo;
        }
    }
}
