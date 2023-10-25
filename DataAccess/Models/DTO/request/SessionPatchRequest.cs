using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.request
{
    public class SessionPatchRequest
    {
        public int schedule_id { get; set; }
        public int? remote_learning { get; set; }
        public int? ass_defense { get; set; }
        public int? eos_exam { get; set; }
        public int? video_learning { get; set; }
        public int? IVQ { get; set; }
        public int? online_lab { get; set; }
        public int? online_test { get; set; }
        public int? assigment { get; set; }
    }
}
