using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class Question
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int question_id { get; set; }
        [Required] 
        public string question_name { get; set; }
        [Required]
        public string question_type { get; set;}
        [ForeignKey("Quiz")]
        public int quiz_id { get; set; }
        [Required]
        public string answers_1 { get; set; }
        [Required]
        public string answers_2 { get; set; }
        [Required]
        public string answers_3 { get; set; }
        [Required]
        public string answers_4 { get; set; }
        [Required]
        public string correct_answer { get; set; }

        public virtual Quiz Quiz { get; set; }
    }
}
