using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.request
{
    public class QuestionDTORequest
    {
        public string question_name { get; set; }
        public string question_type { get; set; }
        public int quiz_id { get; set; }
        public string answers_1 { get; set; }
        public string answers_2 { get; set; }
        public string? answers_3 { get; set; }
        public string? answers_4 { get; set; }
        public string correct_answer { get; set; }
    }
}
