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
    public class CurriculumComboDAO
    {
        private readonly CMSDbContext _context = new CMSDbContext();

        public List<ComboCurriculum> GetListComboByCurriculum(int curriculumId)
        {
            var curriculumCombo = _context.ComboCurriculum
                .Include(x => x.Curriculum)
                .Include(x => x.Combo)
                .Where(x => x.curriculum_id == curriculumId)
                .ToList();
            return curriculumCombo;
        }

        public List<ComboSubject> GetListSubjectByCombo(int comboId)
        {
            var comboSubject = _context.ComboSubject
                .Include(x => x.Combo)
                .Include (x => x.Subject)
                .Where(x => x.combo_id == comboId)
                .ToList();
            return comboSubject;
        }

       

        public string EditCurriculumComboSubject(ComboSubject comboSubject)
        {
            try
            {
                _context.ComboSubject.Update(comboSubject);
                int result = _context.SaveChanges();
                if(result > 0)
                {
                    return Result.updateSuccessfull.ToString();
                }
                else
                {
                    return "Update Fail";
                }
            }catch (DbUpdateConcurrencyException ex)
            {
                return ex.Message;
            }
        }


    }
}
