using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.Excel
{
    public class GradingStrutureExcel
    {
        [DisplayName("#")]

        public int No { get; set; }

        [DisplayName("Assessment Component" + "\nHạng mục đánh giá")]

        public string assessment_component { get; set; }
        [DisplayName("Assessment Type")]

        public string assessment_type { get; set; }
        [DisplayName("Weight" + "\nTrọng số %")]


        public decimal weight { get; set; }

        [DisplayName("Part" + "\nPhần")]

        public int Part { get; set; }
        [DisplayName("Minimun value to meet Completion")]

        public int minimun_value_to_meet { get; set; }
        public string Duration { get; set; }

        public string CLO { get; set; }
        [DisplayName("Type of questions")]

        public string type_of_questions { get; set; }
        [DisplayName("Number of questions")]

        public string number_of_questions { get; set; }
        [DisplayName("Scope of knowledge and skill of questions")]

        public string scope { get; set; }
        [DisplayName("How?")]

        public string how { get; set; }
        public string Note { get; set; }

        public string SessionNo { get; set; }
        public string Reference { get; set; }
    }
}
