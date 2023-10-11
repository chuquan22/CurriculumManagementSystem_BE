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
    public class Specialization
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int specialization_id { get; set; }
        [Required]
        public string specialization_code { get; set; }
        [Required]
        public string specialization_name { get; set; }
        [Required]
        public string specialization_english_name { get; set; }
        [ForeignKey("Major")]
        public int major_id { get; set; }
        [ForeignKey("Semester")]
        public int semester_id { get; set; }
        [Required]
        public bool is_active { get; set; }

        public virtual Major Major { get; set; }

        public virtual ICollection<Combo> Combos { get; set; }

        public virtual ICollection<Curriculum> Curriculums { get; set; }
        public virtual ICollection<SpecializationSubject> SpecializationSubjects { get; set; }
        public virtual Semester Semester { get; set; }

    }
}
