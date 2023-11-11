using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.request
{
    public class SemesterRequest
    {
        [Required]
        public string semester_name { get; set; }
        
        public DateTime semester_start_date { get; set; }
        
        public DateTime semester_end_date { get; set; }
        [Required]
        public int school_year { get; set; }
        [Required]
        public int degree_level_id { get; set; }
        [Required]
        public string batch_name { get; set; }
        [Required]
        public int batch_order { get; set; }
        //[Required]
        //public List<int> list_curriculum_id { get; set; }
    }
}
