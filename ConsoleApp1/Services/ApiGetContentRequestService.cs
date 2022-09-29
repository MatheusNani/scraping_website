using ConsoleApp1.Interfaces;
using ConsoleApp1.Model;
using Microsoft.Extensions.Configuration;

namespace ConsoleApp1.Services
{
    public class ApiGetContentRequestService : IApiGetContentRequestService
    {

        private readonly IConfiguration _config;


        public ApiGetContentRequestService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<HttpResponseMessage> GetSiteContentAsync()
        {
            var apiRequest = new ApiRequestService();

            return await apiRequest.MakeRequestAsync(new RequestParametersModel
            {
                APIBaseAddress = _config.GetValue<string>("UrlApiBase"),
                APIMethodAddress = "/probe/login",
                Method = HttpMethod.Post,
                Headers = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("username", "test"),
                    new KeyValuePair<string, string>("password", "test")
                }
            });
        }
    }
}
