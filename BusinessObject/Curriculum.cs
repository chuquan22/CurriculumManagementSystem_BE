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
    public class Curriculum
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int curriculum_id { get; set; }
        [Required]
        public string curriculum_code { get; set; }
        [Required] 
        public string english_curriculum_name { get; set; }
        [AllowNull]
        public string curriculum_description { get; set; }
        [ForeignKey("Specialization")]
        public int specialization_id { get; set; }
        [Required]
        public string vocational_code { get; set; }
        [Required]
        public string vocational_name { get;set; }
        [Required]
        public string english_vocational_name { get;set; }
        [ForeignKey("Batch")]
        public int batch_id { get; set;}
        [Required]
        public int decision_No { get;set; }
        [Required]
        public DateTime approved_date { get; set; }
        [Required]
        public string curriculum_status { get; set; }

        public virtual Batch Batch { get; set; }
        public virtual Specialization Specialization { get; set; }
        public virtual ICollection<Combo> Combos { get; set;}
        public virtual ICollection<PLOs> PLOs { get; set; }
        public virtual ICollection<SemesterPlan> Semesters { get; set; }
        public virtual ICollection<CurriculumSubject> CurriculumSubjects { get; set; }
    }
}
