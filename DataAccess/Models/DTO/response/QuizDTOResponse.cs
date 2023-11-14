using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.response
{
    public class QuizDTOResponse
    {
        public string quiz_name { get; set; }
        public int number_question_single_choice { get; set; }
        public int number_question_mutiple_choice { get; set; }
    }

    public class MajorSubjectDTOResponse
    {
        public int major_id { get; set; }
        public string major_english_name { get; set; }
        public string degree_level_name { get; set; }
        public List<SubjectDTO> listSubjects { get; set; }
    }

    public class SubjectDTO
    {
        public int subject_id { get; set; }
        public string subject_name { get; set; }
    }
}
