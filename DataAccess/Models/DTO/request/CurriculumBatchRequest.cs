using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.request
{
    public class CurriculumBatchRequest
    {
        public string batch_name { get; set; }
        public int batch_order { get; set; }
        public int degree_level_id { get; set; }
        public List<int> list_curriculum_id { get; set; }
    }
}
