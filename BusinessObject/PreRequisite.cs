using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class PreRequisite
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int pre_requisite_id { get; set; }
        [Required]
        public string pre_requisite_name { get; set; }
        [ForeignKey("PreRequisiteType")]
        public int pre_requisite_type_id { get; set; }

        public virtual ICollection<Subject> subjects { get; set; }  
        public virtual PreRequisiteType PreRequisiteType { get; set; }

    }
}
