using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface ISubjectRepository
    {
        List<Subject> GetAllSubject();
        Subject GetSubjectById(int id);
        Subject GetSubjectByName(string name);
        
    }
}
