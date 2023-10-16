using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.response
{
    public class CurriculumSubjectDTO
    {
        public string semester_no { get; set; }
        public List<CurriculumComboDTOResponse> list { get; set; }
    }
}
