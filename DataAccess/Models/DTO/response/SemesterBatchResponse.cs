﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.response
{
    public class SemesterBatchResponse
    {
        public int semester_batch_id { get; set; }
        public int semester_id { get; set; }
        public int batch_id { get; set; }
        public int batch_name { get; set; }

        public int? term_no { get; set; }
        public string degree_level { get; set; }
    }
}
