using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class GradingCLO
    {
        [ForeignKey("GradingStruture")]
        public int grading_id { get; set; }
        [ForeignKey("CLO")]
        public int CLO_id { get; set;}


        public virtual GradingStruture GradingStruture { get; set; }
        public virtual CLO CLO { get; set;}
    }
}
