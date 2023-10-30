using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.request
{
    public class ClassSessionTypeRequest
    {
        [Required]
        public string class_session_type_name { get; set; }
    }
}
