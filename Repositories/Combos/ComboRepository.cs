﻿using BusinessObject;
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
            return db.CreateCombo(combo);
        }

        public string DeleteCombo(int id)
        {
            return db.DeleteCombo(id);
        }

        public bool DisableCombo(int id)
        {
            return db.DisableCombo(id);
        }

        public Combo FindComboByCode(string comboCode, int speId)
        {
            return db.FindComboByCode(comboCode, speId);
        }

        public Combo FindComboById(int comboId)
        {
            return db.FindComboById(comboId);
        }

        public List<Combo> GetListCombo(int specId)
        {
            return db.GetListCombo(specId);
        }

        public List<Combo> GetListComboByCurriId(int curriId)
        {
            return db.GetListComboByCurriId(curriId);
        } 


        public bool IsCodeExist(string code, int speId)
        {
           return db.IsCodeExist(code,speId);

        }

        public Combo UpdateCombo(Combo combo)
        {
            return db.UpdateCombo(combo);
        }
    }
}
