﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class AssessmentType
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int assessment_type_id { get; set; }
        [Required]
        public string assessment_type_name { get; set; }

        public virtual ICollection<AssessmentMethod>? assessment_methods { get; set;}
    }
}
