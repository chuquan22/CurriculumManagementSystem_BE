using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.request
{
    public class BatchRequest
    {
        [Required]
        public string batch_name { get; set; }
        [Required]
        public int batch_order { get; set; }
        [Required]
        public int degree_level_id { get; set; }
    }
}
