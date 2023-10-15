using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.request
{
    public class SubjectPreRequisiteRequest
    {
        public SubjectRequest SubjectRequest { get; set; }
        public List<PreRequisiteRequest> PreRequisiteRequest { get; set; }
    }
}
