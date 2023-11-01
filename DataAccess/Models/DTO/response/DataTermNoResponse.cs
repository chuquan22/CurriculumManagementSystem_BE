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
        public List<DataSubjectReponse> subjectData { get; set; }

    }
}
