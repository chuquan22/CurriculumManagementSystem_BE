using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.XML
{
    public class Quiz_qti_xml
    {
        public string question_name { get; set; }
        public string question_name_title { get; set; }
        public string question_type { get; set; }
        public List<string> answers { get; set; }
        public List<int> corrects { get; set; }
       
    }
}
