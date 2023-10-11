using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class ComboCurriculum
    {
        [ForeignKey("Combo")]
        public int combo_id { get; set; }
        [ForeignKey("Curriculum")]
        public int curriculum_id { get; set; }

        public virtual Combo Combo { get; set; }
        public virtual Curriculum Curriculum { get; set; }
    }
}
