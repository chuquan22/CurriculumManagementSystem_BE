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
        public string answers_A { get; set; }
        public string answers_B { get; set; }
        public string? answers_C { get; set; }
        public string? answers_D { get; set; }
        public string correct_answer { get; set; }
    }
}
