using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.CurriculumCombo
{
    public interface ICurriculumComboRepository
    {
        List<ComboCurriculum> GetListComboByCurriculum(int curriculumId);
        List<ComboSubject> GetListSubjectByCombo(int comboId);
        string EditCurriculumComboSubject(ComboSubject comboSubject);
    }
}
