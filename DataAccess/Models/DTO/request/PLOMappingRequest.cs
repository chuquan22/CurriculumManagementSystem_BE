using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.request
{
    public class PLOMappingRequest
    {
        public int subject_id {  get; set; }
        public string subject_name { get; set;}
        public Dictionary<string, bool> PLOs { get; set; }
    }
}
