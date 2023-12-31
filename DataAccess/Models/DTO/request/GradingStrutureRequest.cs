﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.request
{
    public class GradingStrutureRequest
    {

        public string? type_of_questions { get; set; }
        public string? number_of_questions { get; set; }

        public int? session_no { get; set; }
        public string? references { get; set; }
        public decimal? grading_weight { get; set; }
        public int? grading_part { get; set; }
        public int? syllabus_id { get; set; }
        
        public int? minimum_value_to_meet_completion { get; set; }
        public string? grading_duration { get; set; }
        public string? scope_knowledge { get; set; }
        public string? how_granding_structure { get; set; }

        public string assessment_component { get; set; }

        public string assessment_type { get; set; }

        public string? grading_note { get; set; }

        public string? clo_name { get; set; }
    }
}
