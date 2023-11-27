using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.response
{
    public class SemesterPlanSubject
    {
        public string speName { get; set; }
        public List<DataTermNoResponse> DataTermNoResponse { get;set; }
    }
}
