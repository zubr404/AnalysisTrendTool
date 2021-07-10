using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace AnalisesTrendTool.ViewModels
{
    /// <summary>
    /// Результат анализа тренда
    /// </summary>
    public class AnaliseTrendResultView
    {
        public string Symbol { get; set; }
        public string Intreval { get; set; }

        private string trendStatus;
        public string TrendStatus 
        {
            get { return trendStatus; }
            set
            {
                trendStatus = value;
                if (trendStatus.Contains(MarketStatistics.TrendStatus.Uptrend.ToString()))
                {
                    ForeGround = Brushes.Green;
                }
                else if (trendStatus.Contains(MarketStatistics.TrendStatus.DownTrend.ToString()))
                {
                    ForeGround = Brushes.Red;
                }
                else
                {
                    ForeGround = Brushes.Gray;
                }
            }
        }
        public int CountPeriodTrendStatus { get; set; }
        public SolidColorBrush ForeGround { get; set; }
    }
}
