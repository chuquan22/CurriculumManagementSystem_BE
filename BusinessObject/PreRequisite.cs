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
    public class PreRequisite
    {
        [ForeignKey("Subject")]
        public int subject_id { get; set; }
        [ForeignKey("PreSubject")]
        public int pre_subject_id { get; set; }
        [ForeignKey("PreRequisiteType")]
        public int pre_requisite_type_id { get; set; }

        public virtual Subject Subject { get; set; }  
        public virtual Subject PreSubject { get; set; }  
        public virtual PreRequisiteType PreRequisiteType { get; set; }

    }
}
