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
        [Required]
        public int batch_order { get; set; }
        //[ForeignKey("DegreeLevel")]
        //public int degree_level_id { get; set; }
        
        public virtual ICollection<CurriculumBatch> CurriculumBatchs { get; set;}
        //public virtual DegreeLevel DegreeLevel { get; set; }
       
    }
}
