using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace DataAccess.Models.DTO.excel
{
    public class ScheduleExcel
    {
        [DisplayName("Sess.")]

        public int session_no { get; set; }
        [DisplayName("Learning-Teaching Method")]

        public string learning_teaching_method { get; set; }
        [DisplayName("Content")]

        public string content { get; set; }
        [DisplayName("CLO")]

        public string CLOs { get; set; }
        [DisplayName("ITU")]

        public string ITU { get; set; }
        [DisplayName("Student's materials")]
        public string student_materials { get; set; }
        [DisplayName("Student's task")]

        public string student_tasks { get; set; }
        [DisplayName("Lecturer's Materials")]

        public string lecturer_materials { get; set; }
        [DisplayName("Lecturer's task")]

        public string lecturer_task { get; set; }
        [DisplayName("Student's materials link")]

        public string student_materials_link { get; set; }
        [DisplayName("Lecturer's materials link")]

        public string lecturer_materials_link { get; set; }
    }
}
