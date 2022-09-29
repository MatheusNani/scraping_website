using ConsoleApp1.Interfaces;
using ConsoleApp1.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Timers;

namespace ConsoleApp1.Services
{
    public class ExecuteService : IExecuteService
    {
        private static System.Timers.Timer _aTimer;
        private static ICollection<SiteDataModel> _dataStorage = new List<SiteDataModel>();

        private readonly IApiGetContentRequestService _apiGetContentRequestService;
        private readonly ICalculateService _calculateService;
        private readonly ILogger<ExecuteService> _log;
        private readonly IConfiguration _config;

        public ExecuteService(
            ILogger<ExecuteService> log,
            IConfiguration config,
            ICalculateService calculateService,
            IApiGetContentRequestService apiGetContentRequestService)
        {
            _log = log;
            _config = config;
            _calculateService = calculateService;
            _apiGetContentRequestService = apiGetContentRequestService;
        }

        public void Run()
        {
            StartContentTimer(_config.GetValue<int>("StartContentTimer"));
            StartCanculatedInfoTimer(_config.GetValue<int>("StartCanculatedInfoTimer"));

            _log.LogInformation("\nPress the Enter key to exit the application...\n");

            Console.WriteLine("The application is running...\n");

            Console.ReadLine();
            _aTimer.Stop();
            _aTimer.Dispose();

            Console.WriteLine("Terminating the application...");
        }

        #region Content Timer
        private void StartContentTimer(int time)
        {
            // Create a timer with 1 second interval.
            _aTimer = new System.Timers.Timer(time);
            _aTimer.Elapsed += OnContentTimeEvent;
            _aTimer.AutoReset = true;
            _aTimer.Enabled = true;
        }

        private void OnContentTimeEvent(Object source, ElapsedEventArgs e)
        {
            try
            {
                var response = _apiGetContentRequestService.GetSiteContentAsync().Result;

                if (response.StatusCode != HttpStatusCode.OK)
                    throw new Exception($"Error: {response.StatusCode} - {response.ReasonPhrase}");

                var rawContent = response.Content.ReadAsStringAsync().Result;

                //Console.WriteLine(rawContent.GetContent("data-ts"));

                var content = rawContent.GetContent("data-ts");

                _dataStorage.Add(new SiteDataModel
                {
                    WaveHeight = content.ToNumber(),
                    SensorTimestamp = content.ConvertSensorTimestamp(),
                    Type = content.GetNumberType()
                });
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
            }
        }
        #endregion

        #region Calculate Timer
        private void StartCanculatedInfoTimer(int time)
        {
            // Create a timer with 15 seconds interval.
            _aTimer = new System.Timers.Timer(time);
            _aTimer.Elapsed += OnCanculatedIntoEvent;
            _aTimer.AutoReset = true;
            _aTimer.Enabled = true;
        }

        private void OnCanculatedIntoEvent(Object source, ElapsedEventArgs e)
        {
            var date = DateTime.Now;

            #region Info

            // get only the last 60sec of data by timestamp not the last 60sec of observed data
            var infoData = _dataStorage.OrderBy(o => o.SensorTimestamp).ToList().Where(w => w.SensorTimestamp > date.AddSeconds(-60)).ToList();

            var average = _calculateService.Avarege(infoData);
            var max = _calculateService.Max(infoData);
            var min = _calculateService.Min(infoData);

            Console.WriteLine($"{date.ToString("yyyy-MM-dd hh:mm:ss")} -- 60s rolling avarage: {average}cm max: {max.MaxWaveHeight}{max.NumberType} min: {min.MaxWaveHeight}{min.NumberType}\n");

            Console.WriteLine($"{date.ToString("yyyy-MM-dd hh:mm:ss")} -- overall avarage: {average}cm max: {max.MaxWaveHeight}{max.NumberType} min: {min.MaxWaveHeight}{min.NumberType}\n");

            #endregion

            #region Warning

            var warningData = _dataStorage.OrderBy(o => o.SensorTimestamp).ToList().Where(w => w.SensorTimestamp > date.AddSeconds(-15)).ToList();

            // get only the last 15sec of data by timestamp not the last 15sec of observed data
            var warning = _calculateService.GenerateWarningInfo(warningData, date);

            if (warning != null)
            {
                Console.WriteLine($"{warning.SensorTimestamp.ToString("G")} *warning* sample was greater than 1m ({warning.MaxWaveHeight})\n");
            }
            #endregion

        }
        #endregion
    }
}