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
    public class User
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int user_id { get; set; }
        [Required, MaxLength(50)]
        public string user_name { get; set; }
        [Required]
        public string user_email { get; set;}
        [Required, MaxLength(255)]
        public string full_name { get; set; }
        [ForeignKey("Role")]
        public int role_id { get; set; }

        [Required]
        public bool is_active { get; set; }
        [AllowNull]
        public string? refresh_token { get; set; }

        public virtual Role Role { get; set; }  
    }
}
