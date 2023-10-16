using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class SyllabusDAO
    {
        public List<Syllabus> GetListSyllabus(int start, int end, string txtSearch)
        {
            List<Syllabus> rs = new List<Syllabus>();
            using (var context = new CMSDbContext())
            {
                rs = context.Syllabus
                                   .Include(s => s.Subject)
                                   .Where(s => s.syllabus_status == 0)
                                   .ToList();
                if (!string.IsNullOrEmpty(txtSearch))
                {
                    rs = rs.Where(sy => sy.Subject.subject_name.Contains(txtSearch)
                    || sy.Subject.subject_code.Contains(txtSearch) )
                    .ToList();                
                }
                rs = rs
                .Skip(start).Take(end).ToList();
            }
            return rs;
        }

        public int GetTotalSyllabus(string txtSearch)
        {
            int rs = 0;
            using (var context = new CMSDbContext())
            {
                rs = context.Syllabus.ToList().Count();
                if (!string.IsNullOrEmpty(txtSearch))
                {
                    rs = context.Syllabus.Include(s => s.Subject).Where(sy => sy.Subject.subject_name.Contains(txtSearch)
                    || sy.Subject.subject_code.Contains(txtSearch)
                    || sy.syllabus_note.Equals(txtSearch)).ToList().Count();
                }
            }
            return rs;
        }

        public Syllabus GetSyllabusById(int id)
        {
            Syllabus rs = new Syllabus();
            using (var context = new CMSDbContext())
            {
                rs = context.Syllabus.Where(s => s.subject_id == id).FirstOrDefault();
            }
            return rs;
        }

    }
}
