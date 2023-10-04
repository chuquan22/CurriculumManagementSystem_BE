namespace CurriculumManagementSystemWebAPI.Models.DTO.response
{
    public class BaseResponse
    {       

        public bool error { get; set; }
        public string message { get; set; }

        public object data { get; set; }
        public BaseResponse()
        {
        }

        public BaseResponse(bool error, String message)
        {
            this.error = error;
            this.message = message;
        }

        public BaseResponse(bool error, String message, Object data)
        {
            this.error = error;
            this.message = message;
            this.data = data;
        }

        public bool isError()
        {
            return error;
        }

        public void setError(bool error)
        {
            this.error = error;
        }

        public String getMessage()
        {
            return message;
        }

        public void setMessage(String message)
        {
            this.message = message;
        }

        public Object getData()
        {
            return data;
        }

        public void setData(Object data)
        {
            this.data = data;
        }
    }
}
