using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class Quiz
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int quiz_id { get; set; }
        [Required] 
        public string quiz_name { get; set; }
        [ForeignKey("Subject")]
        public int subject_id { get; set; }
        public virtual ICollection<Question> Questions { get; set; }

        public virtual Subject Subject { get; set; }
    }
}
