using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace DataAccess.Models.DTO.excel
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
