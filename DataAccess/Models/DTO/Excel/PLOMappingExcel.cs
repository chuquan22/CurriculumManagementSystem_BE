using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.Excel
{
    internal class PLOMappingExcel
    {
        public string subject_code { get; set; }
        public Dictionary<string, bool> PLOs { get; set; }
    }
}
