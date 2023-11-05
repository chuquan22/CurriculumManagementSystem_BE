using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.Report
{
    public class TextBookReport
    {
        public string specialization_name { get; set; }
        public int total_subject { get; set; }
        public int self_edited { get; set; }
        public int open_source_internet { get; set; }
        public int free_ebook { get; set; }
        public int official_publication { get; set; }
        public int books_bought_outside { get; set; }
        public int None { get; set; }

    }
}
