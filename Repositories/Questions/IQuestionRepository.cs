using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Questions
{
    public interface IQuestionRepository
    {
        List<Question> GetQuestionByQuiz(int quizId);
        Question GetQuestionById(int id);
        bool CheckQuestionDuplicate(int id, string questionName, int quizId);
        string CreateQuestion(Question question);
        string UpdateQuestion(Question question);
        string DeleteQuestion(Question question);
    }
}
