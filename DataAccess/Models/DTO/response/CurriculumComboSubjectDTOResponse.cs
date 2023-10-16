using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace DataAccess.Models.DTO.response
{
    public class CurriculumComboSubjectDTOResponse
    {
        public int subject_id { get; set; }
        public string subject_name { get; set; }
        public int combo_id { get; set; }
        public int semester_no { get; set; }

       
    }
}
