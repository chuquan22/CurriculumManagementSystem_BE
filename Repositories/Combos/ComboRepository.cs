using BusinessObject;
using DataAccess.Combos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Combos
{
    public class ComboRepository : IComboRepository
    {
        public ComboDAO db = new ComboDAO();

        public Combo CreateCombo(Combo combo)
        {
            throw new NotImplementedException();
        }

        public void DeleteCombo(Combo combo)
        {
            throw new NotImplementedException();
        }

        public Combo FindComboById(int comboId)
        {
            throw new NotImplementedException();
        }

        public List<Combo> GetListCombo(int specId)
        {
            throw new NotImplementedException();
        }

        public Combo UpdateCombo(Combo combo)
        {
            throw new NotImplementedException();
        }
    }
}
