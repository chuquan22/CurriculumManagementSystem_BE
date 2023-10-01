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
    public class LearningMethod
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int learning_method_id { get; set; }
        [Required]
        public string learning_method_code { get; set; }
        [Required]
        public string learning_method_name { get; set; }
        [AllowNull]
        public string? learning_method_description { get; set; }

        public virtual ICollection<Subject> subjects { get; set; }
    }
}
