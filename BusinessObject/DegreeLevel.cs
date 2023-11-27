using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class DegreeLevel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int degree_level_id { get; set; }
        [Required]
        public string degree_level_code { get; set;}
        [Required]
        public string degree_level_name { get; set; }
        [Required]
        public string degree_level_english_name { get; set; }

        public virtual ICollection<Major>? Majors { get; set; }
        public virtual ICollection<Syllabus>? Syllabus { get; set; }
        public virtual ICollection<Batch>? Batches { get; set; }
    }
}
