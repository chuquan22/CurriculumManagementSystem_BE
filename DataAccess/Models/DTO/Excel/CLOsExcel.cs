using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.Excel
{
    public class CLOsExcel
    {
        public int No { get; set; }
        [DisplayName("CLO Name")]
        public string CLO_Name { get; set; }
        [DisplayName("CLO Description")]

        public string CLO_Description { get; set; }
    }
}
