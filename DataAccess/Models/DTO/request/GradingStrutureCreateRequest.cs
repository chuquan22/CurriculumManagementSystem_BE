using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.request
{
    public class GradingStrutureCreateRequest
    {

        public GradingStrutureRequest gradingStruture { get; set; }
        public GradingCLORequest gradingCLORequest { get; set; }

    }
}
