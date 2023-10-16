using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.response
{
    public class SyllabusDetailsResponse
    {
        public Syllabus syllabus_details { get; set; }
        public Material material_details { get; set; }
        public CLO CLO_details { get; set; }
        public Session session_details { get; set; }
        public GradingStruture gradingStruture_details { get; set; }

    }
}
