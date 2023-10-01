using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class LearningResource
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int learning_resource_id { get; set; }
        [Required]
        public string learning_resource_type { get; set; }

        public virtual ICollection<Material> Material { get; set; }
    }
}
