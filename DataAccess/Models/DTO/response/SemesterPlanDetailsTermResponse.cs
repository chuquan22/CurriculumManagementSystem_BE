﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.response
{
    public class SemesterPlanDetailsTermResponse
    {
        public string specialization_name { get; set; }
        public string major_name { get; set; }
        public int specializationId { get; set; }
        public int no { get; set; }
        public List<SemesterPlanBatchResponse> validBatch { get; set; }
        public List<DataTermNoResponse> courses { get; set; }
    }
}
