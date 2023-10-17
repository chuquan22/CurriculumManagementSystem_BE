using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class GradingStrutureDAO
    {
        private readonly CMSDbContext _cmsDbContext = new CMSDbContext();

        public List<GradingStruture> GetGradingStruture(int id)
        {
            var rs = _cmsDbContext.GradingStruture
                .Where(c => c.syllabus_id == id)
                .ToList();
            return rs;
        }

        public GradingStruture CreateGradingStruture(GradingStruture gra)
        {
            _cmsDbContext.GradingStruture.Add(gra);
            _cmsDbContext.SaveChanges();
            return gra;
        }

        public GradingStruture DeleteGradingStruture(int id)
        {
            var oldGra = _cmsDbContext.GradingStruture.Where(u => u.grading_id == id).FirstOrDefault();
            _cmsDbContext.GradingStruture.Remove(oldGra);
            _cmsDbContext.SaveChanges();
            return oldGra;
        }

        public GradingStruture UpdateGradingStruture(GradingStruture gra)
        {
            var oldGra = _cmsDbContext.GradingStruture.Where(u => u.grading_id == gra.grading_id).FirstOrDefault();
            oldGra.grading_weight = gra.grading_weight;
            oldGra.grading_part = gra.grading_part;
            oldGra.syllabus_id = gra.syllabus_id;
            oldGra.minimum_value_to_meet_completion = gra.minimum_value_to_meet_completion; 
            oldGra.grading_duration = gra.grading_duration;
            oldGra.how_granding_structure = gra.how_granding_structure;
            oldGra.assessment_method_id = gra.assessment_method_id;
            oldGra.grading_note = gra.grading_note;
            _cmsDbContext.GradingStruture.Update(oldGra);
            _cmsDbContext.SaveChanges();
            return oldGra;
        }
    }
}
