using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.request
{
    public class SessionCLOsRequest
    {
        public int CLO_id { get; set; }
        public int? session_id { get; set; }
    }
}
