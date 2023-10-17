using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.excel
{
    public class MaterialExcel
    {
        public int No { get; set; }
        public string MaterialDescription { get; set; }
        public string Purpose { get; set; }

        public string ISBN { get; set; }
        public string Type { get; set; }
        public string Note { get; set; }

        public string Author { get; set; }

        public string Publisher { get; set; }

        public string Published_Date { get; set; }
        public string Edition { get; set; }


    }
}
