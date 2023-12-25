using BusinessObject;
using DataAccess.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class QuestionDAO
    {
        private readonly CMSDbContext _context = new CMSDbContext();

        public List<Question> GetQuestionByQuiz(int quizId)
        {
            var listQuestion = _context.Question.Include(x => x.Quiz.Subject).Where(x => x.quiz_id == quizId).ToList();
            return listQuestion;
        }

        public Question GetQuestionById(int id)
        {
            var question = _context.Question.FirstOrDefault(x => x.question_id == id);
            return question;
        }

        public bool CheckQuestionDuplicate(int id, string questionName, int quizId)
        {
            return (_context.Question?.Any(x => x.question_id != id && x.question_name.Trim().ToLower().Equals(questionName.Trim().ToLower()) && x.quiz_id == quizId)).GetValueOrDefault();
        }
        public Question CheckQuestionDuplicate(string questionName, int quizId)
        {
            return _context.Question.Where(x => x.question_name.Trim().ToLower().Equals(questionName.Trim().ToLower()) && x.quiz_id == quizId).FirstOrDefault();
        }
        public bool CheckAnswerDuplicate(string answerA, string answerB, string answerC, string answerD)
        {
            // Put the answers into a collection
            var answers = new List<string> { answerA.Trim().ToLower(), answerB.Trim().ToLower() };
            if(!string.IsNullOrEmpty(answerC))
            {
                answers.Add(answerC.Trim().ToLower());
            }
            if (!string.IsNullOrEmpty(answerD))
            {
                answers.Add(answerD.Trim().ToLower());
            }
            // Check for duplicates within the provided answers
            if (answers.Distinct().Count() != answers.Count)
            {
                // If the count of distinct answers is less than the total count, there are duplicates
                return true;
            }

            // If no duplicates are found, return false
            return false;
        }

        public string CreateQuestion(Question question)
        {
            try
            {
                // Check for duplicate question
                if (CheckQuestionDuplicate(question.question_name, question.quiz_id) != null)
                {
                    return "Question " + question.question_name + " is Duplicate!";
                }

                // Check for duplicate answers
                if (CheckAnswerDuplicate(question.answers_A, question.answers_B, question.answers_C, question.answers_D))
                {
                    return "Answer is Duplicate!";
                }

                // If no duplicates are found, add the question to the database
                _context.Question.Add(question);
                _context.SaveChanges();

                // Return success message
                return Result.createSuccessfull.ToString();
            }
            catch (Exception ex)
            {
                // Return the error message from the caught exception
                return ex.InnerException.Message;
            }
        }

        public string UpdateQuestion(Question question)
        {
            try
            {
                _context.Question.Update(question);
                _context.SaveChanges();
                return Result.updateSuccessfull.ToString();
            }
            catch (Exception ex)
            {
                return ex.InnerException.Message;
            }
        }

        public string DeleteQuestion(Question question)
        {
            try
            {
                _context.Question.Remove(question);
                _context.SaveChanges();
                return Result.deleteSuccessfull.ToString();
            }
            catch (Exception ex)
            {
                return ex.InnerException.Message;
            }
        }
    }
}
