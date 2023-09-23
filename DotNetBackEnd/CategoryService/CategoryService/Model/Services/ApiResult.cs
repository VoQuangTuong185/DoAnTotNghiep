namespace CategoryService.Model.Services
{
    public class ApiResult
    {
        public ApiResult(object data = null, bool isSuccess = true, string message = "", int httpStatusCode = 200)
        {
            IsSuccess = isSuccess;
            HttpStatusCode = httpStatusCode;
            Message = message;
            Data = data;
        }
        public int HttpStatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        public void InternalError(string message = "")
        {
            HttpStatusCode = 500;
            Message = "Internal Server Error! " + message;
            IsSuccess = false;
        }
    }
}
