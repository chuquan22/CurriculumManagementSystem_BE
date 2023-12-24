using BusinessObject;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Questions
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly QuestionDAO _questionDAO = new QuestionDAO();

        public bool CheckAnswerDuplicate(string answerA, string answerB, string answerC, string answerD)
        {
            return _questionDAO.CheckAnswerDuplicate(answerA, answerB, answerC, answerD);
        }

        public bool CheckQuestionDuplicate(int id, string questionName, int quizId)
        {
            return _questionDAO.CheckQuestionDuplicate(id, questionName, quizId);
        }

        public string CreateQuestion(Question question)
        {
            return _questionDAO.CreateQuestion(question);
        }

        public string DeleteQuestion(Question question)
        {
            return _questionDAO.DeleteQuestion(question);
        }

        public Question GetQuestionById(int id)
        {
            return _questionDAO.GetQuestionById(id);
        }

        public List<Question> GetQuestionByQuiz(int quizId)
        {
            return _questionDAO.GetQuestionByQuiz(quizId);
        }

        public string UpdateQuestion(Question question)
        {
            return _questionDAO.UpdateQuestion(question);
        }
    }
}
