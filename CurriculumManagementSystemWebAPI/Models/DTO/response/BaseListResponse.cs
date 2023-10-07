namespace CurriculumManagementSystemWebAPI.Models.DTO.response
{
    public class BaseListResponse
    {
        public int page { get; set; }
        public int limit { get; set; }
        public object data { get; set; }

        public BaseListResponse()
        {
        }

        public BaseListResponse(int page, int limit, Object data)
        {
            this.page = page;
            this.limit = limit;
            this.data = data;
        }
    }
}
