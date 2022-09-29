using ConsoleApp1.Model;

namespace ConsoleApp1.Interfaces
{
    public interface ICalculateService
    {
        double Avarege(ICollection<SiteDataModel> data);
        InfoModel Min(ICollection<SiteDataModel> data);
        InfoModel Max(ICollection<SiteDataModel> data);
        InfoModel GenerateWarningInfo(ICollection<SiteDataModel> data, DateTime date);
    }
}
