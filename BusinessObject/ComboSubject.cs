using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class ComboSubject
    {
        [ForeignKey("Subject")]
        public int subject_id { get; set; }
        [ForeignKey("Combo")]
        public int combo_id { get; set; }

        public virtual Subject Subject { get; set; }
        public virtual Combo Combo { get; set; }
    }
}
