using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.request
{
    public class SessionPatchUpdateRequest
    {
        public int id { get; set; }
        public SessionUpdateRequest request { get; set; }
    }
}
