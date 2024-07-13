using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiHelper
{
    public interface IInnerApiClient
    {
        Task<BaseResponse<T>> GetAsync<T>(string url);

        Task<BaseResponse<T>> PostAsync<T>(string url);

        Task<BaseResponse<T>> PostAsync<T>(string url, HttpContent? content);

        Task<BaseResponse<T>> PutAsync<T>(string url);

        Task<BaseResponse<T>> PutAsync<T>(string url, HttpContent? content);

        Task<BaseResponse<T>> DeleteAsync<T>(string url);
    }
}
