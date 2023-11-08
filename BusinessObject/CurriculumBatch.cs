using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class CurriculumBatch
    {
        [ForeignKey("Curriculum")]
        public int curriculum_id { get; set; }
        [ForeignKey("Batch")]
        public int batch_id { get; set; }

        public virtual Curriculum Curriculum { get; set; }
        public virtual Batch Batch { get; set; }
    }
}
