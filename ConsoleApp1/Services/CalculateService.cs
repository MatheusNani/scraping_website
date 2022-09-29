using ConsoleApp1.Interfaces;
using ConsoleApp1.Model;
using System;

namespace ConsoleApp1.Services
{
    public class CalculateService : ICalculateService
    {
        public double Avarege(ICollection<SiteDataModel> data)
        {
            if (data.Count <= 0) return 0;

            return Math.Round(data.Sum(s => s.WaveHeight) / data.Count, 2);
        }

        public InfoModel Max(ICollection<SiteDataModel> data)
        {
            if (data.Count <= 0) return null;

            var info = data.MaxBy(m => m.WaveHeight);

            return new InfoModel
            {
                NumberType = info.Type,
                MaxWaveHeight = Math.Round(info.WaveHeight, 2)
            };
        }

        public InfoModel Min(ICollection<SiteDataModel> data)
        {
            if (data.Count <= 0) return null;

            var info = data.MinBy(m => m.WaveHeight);

            return new InfoModel
            {
                NumberType = info.Type,
                MaxWaveHeight = Math.Round(info.WaveHeight, 2)
            };
        }

        /// <summary>
        /// Generate Data from the last SensorTimestamp 15 sec.
        /// </summary>
        /// <param name="data"> Data List </param>
        /// <param name="date"> Date </param>
        /// <returns>WarningInfoModel</returns>
        public InfoModel GenerateWarningInfo(ICollection<SiteDataModel> data, DateTime date)
        {
            var warningData = data.Where(w => w.SensorTimestamp > date.AddSeconds(-15) && w.WaveHeight > 1 && w.Type.Equals("m"))
           .OrderBy(o => o.SensorTimestamp)
           .Select(s => new
           {
               WaveHeight = s.WaveHeight,
               SensorTimestamp = s.SensorTimestamp

           }).ToList();

            if (warningData != null && warningData.Count() > 0)
            {
                var WaveHeightMax = warningData.MaxBy(s => s.WaveHeight);

                return new InfoModel()
                {
                    MaxWaveHeight = WaveHeightMax.WaveHeight,
                    SensorTimestamp = WaveHeightMax.SensorTimestamp
                };
            }

            return null;
        }
    }
}
