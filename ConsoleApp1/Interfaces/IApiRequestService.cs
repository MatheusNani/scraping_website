using ConsoleApp1.Model;

namespace ConsoleApp1.Interfaces
{
    public interface IApiRequestService
    {
        Task<HttpResponseMessage> MakeRequestAsync(RequestParametersModel requestParameters);        
    }
}
