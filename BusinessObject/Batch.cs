using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class Batch
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int batch_id { get; set; }
        [Required] 
        public string batch_name { get; set; }

        public virtual ICollection<Curriculum> Curriculums { get; set;}
    }
}
