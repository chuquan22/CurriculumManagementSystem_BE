namespace DataAccess.Models.DTO.response
{
    public class BaseListResponse
    {
        public int page { get; set; }
        public int limit { get; set; }
        public int total { get; set; }
        public object data { get; set; }

        public BaseListResponse()
        {
        }

        public BaseListResponse(int page, int limit, int total, object data)
        {
            this.page = page;
            this.limit = limit;
            this.total = total;
            this.data = data;
        }
    }
}
