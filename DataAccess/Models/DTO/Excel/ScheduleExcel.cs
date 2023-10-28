using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.Excel
{
    public class ScheduleExcel
    {
        [DisplayName("Sess.")]


        public int session_No { get; set; }
        [DisplayName("Leaning-Teaching Method")]

        public string leaning_teaching_method { get; set; }

        public string Content { get; set; }
        public string CLO { get; set; }
        public string ITU { get; set; }
        [DisplayName("Student's materials")]

        public string student_materials { get; set; }
        [DisplayName("Student's task")]

        public string student_task { get; set; }
        [DisplayName("Lecturer's Materials")]

        public string lecture_materials { get; set; }
        [DisplayName("Lecturer's task")]

        public string lecture_task { get; set; }
        [DisplayName("Student's materials link")]

        public string student_material_link { get; set; }
        [DisplayName("Lecturer's Materials link")]

        public string lecture_material_link { get; set; }
    }
}
