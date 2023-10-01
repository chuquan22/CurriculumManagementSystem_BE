using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class SpecializationSubject
    {
        [ForeignKey("Specialization")]
        public int specialization_id { get; set; }
        [ForeignKey("Subject")]
        public int subject_id { get; set; }

        public virtual Specialization Specialization { get; set; }  
        public virtual Subject Subject { get; set; }
    }
}
