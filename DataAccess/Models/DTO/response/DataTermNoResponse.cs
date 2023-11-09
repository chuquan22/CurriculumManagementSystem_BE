using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.response
{
    public class DataTermNoResponse
    {
        public int? term_no { get; set; }
        public string? batch { get;set; }
        public int? batch_order { get; set; }
        public string? batch_check { get;set; }
        public string? curriculum_code { get; set; }
        public List<DataSubjectReponse> subjectData { get; set; }

    }
}
