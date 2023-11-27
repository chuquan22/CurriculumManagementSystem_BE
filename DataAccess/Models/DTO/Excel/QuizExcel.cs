using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.Excel
{
    public class QuizExcel
    {
        public int NO { get; set; }
        public string QUESTION { get; set; }
        public string ABC { get; set; }
        public string ANSWER { get; set; }
        public string CORRECT { get; set; }
    }
}
