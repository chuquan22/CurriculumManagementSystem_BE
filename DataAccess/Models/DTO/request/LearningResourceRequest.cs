using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.request
{
    public class LearningResourceRequest
    {
        [Required]
        public string learning_resource_type { get; set; }
    }
}
