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

        public Combo CreateCombo(Combo combo);
        public Combo UpdateCombo(Combo combo);
        public Combo DeleteCombo(int id);
        public Combo FindComboById(int comboId);

    }
}
