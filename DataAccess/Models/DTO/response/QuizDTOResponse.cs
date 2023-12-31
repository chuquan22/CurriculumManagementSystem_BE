﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.response
{
    public class QuizDTOResponse
    {
        public int quiz_id { get; set; }
        public string quiz_name { get; set; }
        public int number_question_single_choice { get; set; }
        public int number_question_mutiple_choice { get; set; }
    }

    public class QuizResponse
    {
        public int quiz_id { get; set; }
        public string quiz_name { get; set; }
        public int subject_id { get; set; }
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
        public string subject_code { get; set; }
    }

    public class QuestionDTOResponse
    {
        public int major_id { get; set; }
        public int subject_id { get; set; }
        public List<QuestionResponse> questionResponses { get; set; }
    }

    public class QuestionResponse
    {
        public int question_id { get; set; }
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
