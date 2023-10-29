using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.request
{
    public class AssessmentMethodRequest
    {
        public string assessment_method_component { get; set; }
        public int assessment_type_id { get; set; }
    }
}
