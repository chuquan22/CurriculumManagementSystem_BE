using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class PLOs
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PLO_id { get; set; }
        [Required] 
        public string PLO_name { get; set; }
        [ForeignKey("Curriculum")]
        public int curriculum_id { get; set; }
        
        [AllowNull] 
        public string PLO_description { get; set; }

        public virtual Curriculum Curriculum { get; set; }
        public virtual ICollection<PLOMapping> PLOMappings { get; set; }

    }
}
