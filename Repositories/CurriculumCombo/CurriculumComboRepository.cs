using BusinessObject;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.CurriculumCombo
{
    public class CurriculumComboRepository : ICurriculumComboRepository
    {
        private readonly CurriculumComboDAO curriculumComboDAO = new CurriculumComboDAO();
        public string EditCurriculumComboSubject(ComboSubject comboSubject)
        {
            return curriculumComboDAO.EditCurriculumComboSubject(comboSubject);
        }

        public List<ComboCurriculum> GetListComboByCurriculum(int curriculumId)
        {
            return curriculumComboDAO.GetListComboByCurriculum(curriculumId);
        }

        public List<ComboSubject> GetListSubjectByCombo(int comboId)
        {
            return curriculumComboDAO.GetListSubjectByCombo(comboId);
        }
    }
}
