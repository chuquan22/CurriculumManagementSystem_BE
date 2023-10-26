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
        public float? remote_learning { get; set; }
        public float? ass_defense { get; set; }
        public float? eos_exam { get; set; }
        public float? video_learning { get; set; }
        public float? IVQ { get; set; }
        public float? online_lab { get; set; }
        public float? online_test { get; set; }
        public float? assigment { get; set; }
    }
}
