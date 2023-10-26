using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class GradingStruture
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int grading_id { get; set; }
        [AllowNull]
        public string? type_of_questions { get; set; }
        [AllowNull]
        public string? number_of_questions { get; set; }
        [AllowNull]

        public int? session_no { get; set; }
        [AllowNull]
        public string? references { get; set; }
        [Required]
        public decimal grading_weight { get; set; }
        [AllowNull]
        public int grading_part { get; set; }
        [ForeignKey("Syllabus")]
        public int syllabus_id { get; set; }
        [AllowNull]
        public int? minimum_value_to_meet_completion { get; set; }
        [AllowNull]
        public string? grading_duration { get; set; }
        [AllowNull]
        public string? scope_knowledge { get; set; }
        [AllowNull]
        public string? how_granding_structure { get; set;}
        [ForeignKey("AssessmentMethod")]
        public int assessment_method_id { get; set; }
        [AllowNull]
        public string? grading_note { get; set; }
        [AllowNull]

        public string? clo_name { get; set; }

        public virtual Syllabus? Syllabus { get; set;}
        public virtual AssessmentMethod? AssessmentMethod { get; set; }
        
        public virtual ICollection<GradingCLO>? GradingCLOs { get; set; }    
    }
}
