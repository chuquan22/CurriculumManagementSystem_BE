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
    public class Syllabus
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int syllabus_id { get; set; }
        [Required]
        public string document_type { get; set; }
        [Required]
        public string program { get; set; }
        [Required]
        public string decision_No { get; set; }
        [Required]
        public string degree_level { get; set; }
        [Required]
        public string syllabus_description { get; set; }
        [ForeignKey("Subject")]
        public int subject_id { get; set; }
        [Required]
        public string student_task { get; set; }
        [AllowNull]
        public string? syllabus_tool { get; set; }
        [AllowNull]
        public string? syllabus_note { get; set; }
        [Required]
        public decimal min_GPA_to_pass { get; set; }
        [Required]
        public int scoring_scale { get; set; }
        [AllowNull]
        public DateTime? approved_date { get; set; }
        [Required]
        public bool syllabus_status { get; set; }
        [Required]
        public bool syllabus_approved { get; set; }

        public virtual ICollection<CLO>? CLOs { get; set; }
        public virtual ICollection<GradingStruture>? Gradings { get; set; }
        public virtual ICollection<Material>? Materials { get; set; }
        public virtual Subject? Subject { get; set; }
        public virtual ICollection<Session>? Sessions { get; set; }
    }
}
