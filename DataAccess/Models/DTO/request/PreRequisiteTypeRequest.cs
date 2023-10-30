using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.request
{
    public class PreRequisiteTypeRequest
    {
        [Required]
        public string pre_requisite_type_name { get; set; }
    }
}
