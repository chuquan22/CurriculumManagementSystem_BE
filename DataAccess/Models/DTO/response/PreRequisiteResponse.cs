using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.response
{
    public class PreRequisiteResponse
    {
        public int subject_id { get; set; }
        public int pre_subject_id { get; set; }
        public string subject_code { get; set; }
        public string subject_name { get; set; }
        public int pre_requisite_type_id { get; set; }
        public string pre_requisite_type_name { get; set; }
    }
}
