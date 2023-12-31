﻿using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.response
{
    public class CurriculumBatchDTOResponse
    {
        public int batch_id { get; set; }
        public string batch_name { get; set; }
        public int batch_order { get; set; }
        public int degree_level_id { get; set; }
        public string degree_level_name { get; set; }
        public List<CurriculumResponse> curriculum { get; set; }

    }
}
