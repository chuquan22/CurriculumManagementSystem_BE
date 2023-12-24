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
    public class Semester
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int semester_id { get; set; }
        [Required]
        public string semester_name { get; set; }
        [Required]
        public DateTime semester_start_date { get; set;}
        [Required]
        public DateTime semester_end_date { get; set; }
        [Required]
        public int school_year { get; set; }
        [ForeignKey("Batch")]
        public int start_batch_id { get; set; }
        [AllowNull]
        public int? no { get; set; }

        public virtual ICollection<SemesterPlan> Semesters { get; set; }
        public virtual ICollection<Specialization> Specializations { get; set; }
        public virtual Batch Batch { get; set; }
    }
}
