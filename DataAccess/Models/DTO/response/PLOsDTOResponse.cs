using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.response
{
    public class PLOsDTOResponse
    {
        public int PLO_id { get; set; }
        public string PLO_name { get; set; }
        public string PLO_description { get; set; }
    }
}
