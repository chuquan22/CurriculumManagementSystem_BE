﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class Combo
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int combo_id { get; set; }
        [Required] 
        public string combo_code { get; set; }
        [Required]
        public string combo_name { get; set;}
        [Required]
        public string combo_english_name { get; set;}
        [Required]
        public int combo_no { get; set; }
        [Required]
        public string combo_description { get; set; }
        [ForeignKey("Specialization")]
        public int specialization_id { get; set; }
        [ForeignKey("Curriculum")]
        public int curriculum_id { get; set; }
        [Required]
        public bool is_active { get; set; }

        public virtual Specialization Specialization { get; set; }
        public virtual Curriculum Curriculum { get; set; }
        public virtual ICollection<ComboSubject> comboSubjects { get; set; }   
        public virtual ICollection<ComboCurriculum> ComboCurriculum { get; set; }   
    }
}
