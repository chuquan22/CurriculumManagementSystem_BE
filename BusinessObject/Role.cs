using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class Role
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int role_id { get; set; }
        [Required, MaxLength(50)] 
        public string role_name { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
