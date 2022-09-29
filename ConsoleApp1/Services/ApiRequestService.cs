using ConsoleApp1.Interfaces;
using ConsoleApp1.Model;
using System.Net;

namespace ConsoleApp1.Services
{
    public class ApiRequestService : IApiRequestService
    {
        /// <summary>
        /// Request site information
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        public async Task<HttpResponseMessage> MakeRequestAsync(RequestParametersModel requestParameters)
        {
            if (requestParameters == null) return null;

            // Prepare request
            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler) { BaseAddress = new Uri(requestParameters.APIBaseAddress) })
            {
                var content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>());

                // add headers parameters to make the request
                if (requestParameters.Headers != null && requestParameters.Headers.Any())
                {
                    content = new FormUrlEncodedContent(requestParameters.Headers);
                }

                var result = client.PostAsync(requestParameters.APIMethodAddress, content);

                //result.Result.EnsureSuccessStatusCode();

                return await result;
            }
        }
    }
}
