﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.Report
{
    public class TKOL_DTOReport
    {
        public string batch_name { get; set; }
        public string major_name { get; set; }
        public string learning_method_T01_name { get; set; }
        public string learning_method_T02_name { get; set; }
        public string learning_method_T03_name { get; set; }
        public List<TKOLReport>? tkol { get; set; }
    }
}
