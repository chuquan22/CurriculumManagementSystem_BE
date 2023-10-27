using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace BusinessObject
{
    public class Session
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int schedule_id { get; set; }
        [Required]
        public string schedule_content { get; set; }
        [ForeignKey("Syllabus")]
        public int syllabus_id { get; set; }
        [Required]

        public int session_No { get; set; }
        [AllowNull]

        public string? ITU { get; set; }
        [AllowNull]
        public string? schedule_student_task { get; set; }
        [AllowNull]
        public string? student_material { get; set; }
        [AllowNull]
        public string? lecturer_material { get; set;}
        [AllowNull]
        public string schedule_lecturer_task { get; set; }
        [AllowNull]
        public string? student_material_link { get; set; }
        [AllowNull]
        public string? lecturer_material_link { get; set; }
        [ForeignKey("ClassSessionType")]
        public int class_session_type_id { get; set; }
        //check
        [Required]
        public float remote_learning  { get; set; }
        [Required]
        public float ass_defense { get; set; }
        [Required]
        public float eos_exam { get; set; }
        [Required]
        public float video_learning { get; set; }
        [Required]
        public float IVQ { get; set; }
        [Required]
        public float online_lab { get; set; }
        [Required]
        public float online_test { get; set; }
        [Required]
        public float assigment { get; set; }

        public virtual Syllabus Syllabus { get; set; }
        public virtual ClassSessionType ClassSessionType { get; set; }
        
        public virtual ICollection<SessionCLO> SessionCLO { get; set; } 
    }
}