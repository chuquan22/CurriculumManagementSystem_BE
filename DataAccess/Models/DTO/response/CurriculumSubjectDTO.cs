﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.response
{
    public class CurriculumSubjectDTO
    {
        public string semester_no { get; set; }
        public int total_all_credit { get; set; }
        public int total_all_time { get; set; }
        public List<CurriculumSubjectResponse> list { get; set; }
       
    }
}
