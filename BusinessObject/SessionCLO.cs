using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class SessionCLO
    {
        [ForeignKey("CLO")]
        public int CLO_id { get; set; }
        [ForeignKey("Session")]
        public int session_id { get; set; }
        //cần thêm dữ liệu????

        public virtual CLO CLO { get; set; }
        public virtual Session Session { get; set; }
    }
}
