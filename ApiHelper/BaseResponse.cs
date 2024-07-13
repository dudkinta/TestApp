using CommonLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

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
