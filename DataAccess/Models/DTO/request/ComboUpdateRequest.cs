﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.request
{
    public class ComboUpdateRequest
    {
        public int combo_id { get; set; }
        public string combo_name { get; set; }
        public string combo_english_name { get; set; }
        public bool is_active { get; set; }

    }
}
