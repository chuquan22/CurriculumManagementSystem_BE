using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.response
{
    public class DataSubjectReponse
    {
        public string subject_code { get; set; }
        public string subject_name { get; set; }
        public int credit { get; set; }
        public int total { get; set; }
        public int @class { get; set; }
        public int @exam { get; set; }
        public string method { get; set; }
        public string optional { get; set; }
        public string combo { get; set; }

    }
}
