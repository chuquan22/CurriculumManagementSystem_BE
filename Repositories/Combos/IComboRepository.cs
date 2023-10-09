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
        public List<Combo> GetListCombo(int specId, int currId);

        public Combo CreateCombo(Combo combo);
        public Combo UpdateCombo(Combo combo);
        public void DeleteCombo(Combo combo);
        public Combo FindComboById(int comboId);

    }
}
