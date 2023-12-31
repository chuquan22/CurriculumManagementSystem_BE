﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.response
{
    public class SemesterResponse
    {

        public int semester_id { get; set; }
        public string semester_name { get; set; }
        public string semester_start_date { get; set; }
        public string semester_end_date { get; set; }
        public int school_year { get; set; }
        public int start_batch_id { get; set; }
        public string batch_name { get; set; }
        public int batch_order { get; set; }
        public int degree_level_id { get; set; }
        public string degree_level_name { get; set; }
    }
}
