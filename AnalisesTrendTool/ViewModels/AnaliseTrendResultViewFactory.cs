using BinanceAPI.KlineData;
using MarketStatistics.Trend;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnalisesTrendTool.ViewModels
{
    class AnaliseTrendResultViewFactory
    {
        public AnaliseTrendResultView Create(Klines klines, EMAAnaliseTrend emaAnaliseTrend)
        {
            return new AnaliseTrendResultView()
            {
                Symbol = klines.Simbol,
                Intreval = klines.Interval,
                TrendStatus = emaAnaliseTrend.TrendStatus.ToString(),
                CountPeriodTrendStatus = emaAnaliseTrend.CountPeriodTrendStatus
            };
        }
    }
}
