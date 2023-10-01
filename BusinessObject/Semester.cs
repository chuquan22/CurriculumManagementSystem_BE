using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        public virtual ICollection<SemesterPlan> semesters { get; set; }
    }
}
