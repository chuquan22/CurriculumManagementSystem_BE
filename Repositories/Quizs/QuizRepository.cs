using BusinessObject;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Quizs
{
    public class QuizRepository : IQuizRepository
    {
        private readonly QuizDAO _quizDAO = new QuizDAO();
        public bool CheckQuizDuplicate(string quizName, int subjectId)
        {
            return _quizDAO.CheckQuizDuplicate(quizName, subjectId);
        }

        public bool CheckQuizExsit(int quizId)
        {
            return _quizDAO.CheckQuizExsit(quizId);
        }

        public string CreateQUiz(Quiz quiz)
        {
            return _quizDAO.CreateQUiz(quiz);
        }

        public string DeleteQUiz(Quiz quiz)
        {
            return _quizDAO.DeleteQUiz(quiz);
        }

        public List<Quiz> GetAllQUiz()
        {
            return _quizDAO.GetAllQUiz();
        }

        public Quiz GetQuizById(int id)
        {
            return _quizDAO.GetQuizById(id);
        }

        public List<Quiz> GetQUizBySubjectId(int subjectId)
        {
            return _quizDAO.GetQUizBySubjectId(subjectId);
        }

        public string UpdateQUiz(Quiz quiz)
        {
            return _quizDAO.UpdateQUiz(quiz);
        }
    }
}
