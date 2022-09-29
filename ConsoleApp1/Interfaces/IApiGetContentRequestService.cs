namespace ConsoleApp1.Interfaces
{
    public interface IApiGetContentRequestService
    {
        Task<HttpResponseMessage> GetSiteContentAsync();
    }
}