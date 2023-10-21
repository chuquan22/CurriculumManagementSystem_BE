using BusinessObject;
using DataAccess.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class PLOsDAO
    {
        private readonly CMSDbContext _context = new CMSDbContext();

        public List<PLOs> GetListPLOsByCurriculum(int curriculumId)
        {
            var listPLOs = _context.PLOs
                .Include(x => x.Curriculum)
                .Where(x => x.curriculum_id == curriculumId).ToList();
            return listPLOs;
        }

        public bool CheckPLONameExsit(string ploName, int curriId)
        {
            return (_context.PLOs?.Any(x => x.curriculum_id == curriId && x.PLO_name.Equals(ploName))).GetValueOrDefault();
        }

        public PLOs GetPLOsById(int id)
        {
            var PLO = _context.PLOs.FirstOrDefault(x => x.PLO_id == id);
            return PLO;
        }

        public string CreatePLOs(PLOs plo)
        {
            try
            {
                _context.PLOs.Add(plo);
                int index = _context.SaveChanges();
                if(index > 0)
                {
                    return Result.createSuccessfull.ToString();
                }
                else
                {
                    return "Create PLOs Fail!";
                }

            }catch (DbUpdateConcurrencyException ex)
            {
                return ex.Message;
            }
        }

        public string UpdatePLOs(PLOs plo)
        {
            try
            {
                _context.PLOs.Update(plo);
                int index = _context.SaveChanges();
                if (index > 0)
                {
                    return Result.updateSuccessfull.ToString();
                }
                else
                {
                    return "Update PLOs Fail!";
                }

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string DeletePLOs(PLOs plo)
        {
            try
            {
                _context.PLOs.Remove(plo);
                int index = _context.SaveChanges();
                if (index > 0)
                {
                    return Result.deleteSuccessfull.ToString();
                }
                else
                {
                    return "Delete PLOs Fail!";
                }

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
