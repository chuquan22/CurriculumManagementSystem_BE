using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.Excel
{
    public class PLOMappingExcel
    {
        [DisplayName("Subject Code")]
        public string subject_code { get; set; }
        [DisplayName("PLO")]
        public List<string> PLO { get; set; }
    }
}
