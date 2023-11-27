using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Quizs
{
    public interface IQuizRepository
    {
        List<Quiz> GetAllQUiz();
        List<Quiz> GetQUizBySubjectId(int subjectId);
        Quiz GetQuizById(int id);
        bool CheckQuizDuplicate(string quizName, int subjectId);
        bool CheckQuizExsit(int quizId);
        string CreateQUiz(Quiz quiz);
        string UpdateQUiz(Quiz quiz);
        string DeleteQUiz(Quiz quiz);
    }
}
