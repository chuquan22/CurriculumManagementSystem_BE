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
    public class SemesterBatch
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int semester_batch_id { get; set; }
        [ForeignKey("Semester")]
        public int semester_id { get; set; }
        [ForeignKey("Batch")]
        public int batch_id { get; set; }

        [AllowNull]
        public int? term_no { get; set; }
        [Required]
        public string degree_level { get; set; }
        public virtual Batch Batch { get; set; }
        public virtual Semester? Semester { get; set; }
    }
}
