using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.request
{
    public class SessionRequest
    {
        public string schedule_content { get; set; }
        public int syllabus_id { get; set; }

        public int session_No { get; set; }

        public string ITU { get; set; }
        public string schedule_student_task { get; set; }
        public string student_material { get; set; }
        public string lecturer_material { get; set; }
        public string schedule_lecturer_task { get; set; }
        public string? student_material_link { get; set; }
        public string? lecturer_material_link { get; set; }
        public int class_session_type_id { get; set; }
        //check
        public float remote_learning { get; set; }
        public float ass_defense { get; set; }
        public float eos_exam { get; set; }
        public float video_learning { get; set; }
        public float IVQ { get; set; }
        public float online_lab { get; set; }
        public float online_test { get; set; }
        public float assigment { get; set; }

    }
}
