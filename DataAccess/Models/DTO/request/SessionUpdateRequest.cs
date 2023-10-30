using DataAccess.Models.DTO.response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.request
{
    public class SessionUpdateRequest
    {
        public SessionUpdate session { get; set; }
        public List<SessionCLOsRequest> session_clo { get; set; }
    }
}
