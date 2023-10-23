using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.response
{
    public class SessionResponse
    {
        public int schedule_id { get; set; }

        public string schedule_content { get; set; }
        public int syllabus_id { get; set; }


        public int session_No { get; set; }

        public string ITU { get; set; }
        public long schedule_student_task { get; set; }
        public string student_material { get; set; }
        public string lecturer_material { get; set; }
        public long schedule_lecturer_task { get; set; }
        public string? student_material_link { get; set; }
        public string? lecturer_material_link { get; set; }
        public int class_session_type_id { get; set; }
        //check
        public int remote_learning { get; set; }

        public int ass_defense { get; set; }
        public int eos_exam { get; set; }
        public int video_learning { get; set; }
        public int IVQ { get; set; }

        public int online_lab { get; set; }
        public int online_test { get; set; }
        public int assigment { get; set; }
        public List<ListCLOsResponse> listCLOs { get; set; }
    }
}
