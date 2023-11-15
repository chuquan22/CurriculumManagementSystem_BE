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
    public class QuizDAO
    {
        private readonly CMSDbContext _context = new CMSDbContext();

        public List<Quiz> GetAllQUiz()
        {
            var listQuiz = _context.Quiz.ToList();
            return listQuiz;
        }

        public List<Quiz> GetQUizBySubjectId(int subjectId)
        {
            var listQuiz = _context.Quiz.Where(x => x.subject_id == subjectId).ToList();
            return listQuiz;
        }

        public Quiz GetQuizById(int id)
        {
            var quiz = _context.Quiz.FirstOrDefault(x => x.quiz_id == id);
            return quiz;
        }

        public bool CheckQuizDuplicate(string quizName, int subjectId)
        {
            return (_context.Quiz?.Any(x => x.subject_id == subjectId && x.quiz_name.ToLower().Equals(quizName.ToLower()))).GetValueOrDefault();
        }

        public bool CheckQuizExsit(int quizId)
        {
            return (_context.Question?.Any(x => x.quiz_id == quizId)).GetValueOrDefault();
        }

        public string CreateQUiz(Quiz quiz)
        {
            try
            {
                _context.Quiz.Add(quiz);
                _context.SaveChanges();
                return Result.createSuccessfull.ToString();
            }
            catch (Exception ex)
            {
                return ex.InnerException.Message;
            }
        }

        public string UpdateQUiz(Quiz quiz)
        {
            try
            {
                _context.Quiz.Update(quiz);
                _context.SaveChanges();
                return Result.updateSuccessfull.ToString();
            }
            catch (Exception ex)
            {
                return ex.InnerException.Message;
            }
        }

        public string DeleteQUiz(Quiz quiz)
        {
            try
            {
                _context.Quiz.Remove(quiz);
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
