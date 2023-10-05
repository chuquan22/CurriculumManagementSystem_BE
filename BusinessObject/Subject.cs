using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class Subject
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int subject_id { get; set; }
        [Required]
        public string subject_code { get; set; }
        [Required]
        public string subject_name { get; set;}
        [Required]
        public string english_subject_name { get; set;}
        [ForeignKey("LearningMethod")]
        public int learning_method_id { get; set; }
        
        [ForeignKey("AssessmentMethod")]
        public int assessment_method_id { get;set; }
        [Required]
        public int credit { get; set; }
        
        [Required]
        public string option { get; set; }
        [Required]
        public bool is_active { get; set; }

        public virtual ICollection<Quiz> Quizzes { get; set; }
        public virtual ICollection<Syllabus> Syllabus { get; set;}
        public virtual LearningMethod LearningMethod { get; set; }
        public virtual AssessmentMethod AssessmentMethod { get; set; }
        public virtual ICollection<PreRequisite> PreRequisite { get; set; }
        public virtual ICollection<CurriculumSubject> CurriculumSubjects { get; set; }
        public virtual ICollection<SpecializationSubject> SpecializationSubjects { get; set; }
        public virtual ICollection<ComboSubject> ComboSubjects { get; set; }    
        public virtual ICollection<PLOMapping> PLOMappings { get; set; }    
    }
}
