using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.request
{
    public class MaterialRequest
    {
        public string material_description { get; set; }
        public string? material_purpose { get; set; }
        public string? material_ISBN { get; set; }
        public string material_type { get; set; }
        public int syllabus_id { get; set; }
        public string? material_note { get; set; }
        public int learning_resource_id { get; set; }
        public string? material_author { get; set; }
        public string? material_publisher { get; set; }
        public DateTime? material_published_date { get; set; }
        public string? material_edition { get; set; }
    }
}
