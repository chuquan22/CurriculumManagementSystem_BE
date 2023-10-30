using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Combos
{
    public interface IComboRepository
    {
        public List<Combo> GetListCombo(int specId);
        List<Combo> GetListComboByCurriId(int curriId);
        public Combo CreateCombo(Combo combo);
        public Combo UpdateCombo(Combo combo);
        public string DeleteCombo(int id);
        public Combo FindComboById(int comboId);
        Combo FindComboByCode(string comboCode);
        bool DisableCombo(int id);

        bool IsCodeExist(string code);
    }
}
