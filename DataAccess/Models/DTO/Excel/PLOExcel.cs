using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.Excel
{
    public class PLOExcel
    {
        public int No { get; set; }
        [DisplayName("PLO Name")]
        public string PLO_name { get; set; }
        [DisplayName("PLO Description")]
        public string PLO_description { get; set; }
    }
}
