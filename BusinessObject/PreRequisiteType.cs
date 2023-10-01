using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class PreRequisiteType
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int pre_requisite_type_id { get; set; }
        [Required] 
        public string pre_requisite_type_name { get; set; }

        public virtual ICollection<PreRequisite> PreRequisites { get; set; }
    }
}
