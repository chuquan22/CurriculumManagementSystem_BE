using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.response
{
    public class PaginationResponse<T>
    {
        public int Page { get; set; }
        public int Limit { get; set; }
        public int TotalElements { get; set; }
        public List<T> Data { get; set; }
    }
}
