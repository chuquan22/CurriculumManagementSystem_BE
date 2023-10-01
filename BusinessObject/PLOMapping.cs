using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class PLOMapping
    {
        [ForeignKey("PLOs")]
        public int PLO_id { get; set; }
        [ForeignKey("Subject")]
        public int subject_id { get; set; }

        public virtual PLOs PLOs { get; set; }
        public virtual Subject Subject { get; set; }
    }
}
