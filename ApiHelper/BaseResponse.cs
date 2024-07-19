using CommonLibrary;
using System.Net;

namespace ApiHelper
{
    public class BaseResponse<T>
    {
        public bool IsSuccessStatusCode { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public T? Message { get; set; }
        public ErrorResponse? Error { get; set; }
    }
}
