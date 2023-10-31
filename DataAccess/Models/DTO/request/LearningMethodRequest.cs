using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.request
{
    public class LearningMethodRequest
    {
        [Required]
        public string learning_method_name { get; set; }
        
    }
}
