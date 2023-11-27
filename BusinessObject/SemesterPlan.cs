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
    public class SemesterPlan
    {
        [ForeignKey("Curriculum")]
        public int curriculum_id { get; set; }
        [ForeignKey("Semester")]
        public int semester_id { get; set; }
        [AllowNull]
        public int? term_no { get; set; }

        public virtual Curriculum? Curriculum { get; set; }
        public virtual Semester? Semester { get; set; }
    }
}
