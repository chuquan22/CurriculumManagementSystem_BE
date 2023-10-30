using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.request
{
    public class SessionCreateRequest
    {
        public SessionRequest session { get; set; }
        public List<SessionCLOsRequest> session_clo { get; set; }
    }
}
