﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.request
{
    public class CurriculumUpdateRequest
    {
        [Required]
        public string curriculum_name { get; set; }
        [Required]
        public string english_curriculum_name { get; set; }
        [Required]
        public string curriculum_description { get; set; }
        [AllowNull]
        public string? decision_No { get; set; }

        [AllowNull]
        public DateTime? approved_date { get; set; }

    }
}
