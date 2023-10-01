using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class ClassSessionType
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int class_session_type_id { get; set; }
        [Required] 
        public string class_session_type_name { get; set; }  

        public virtual ICollection<Session> Sessions { get; set; }
    }
}
