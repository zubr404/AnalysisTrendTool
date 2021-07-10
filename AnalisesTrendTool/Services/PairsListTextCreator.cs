using AnalisesTrendTool.ViewModels;
using MarketStatistics;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnalisesTrendTool.Services
{
    /// <summary>
    /// Создает форматировнный техт списка пар
    /// </summary>
    class PairsListTextCreator
    {
        private readonly List<AnaliseTrendResultView> analiseTrendResultViews;

        public PairsListTextCreator(List<AnaliseTrendResultView> analiseTrendResultViews)
        {
            this.analiseTrendResultViews = analiseTrendResultViews;
        }

        private const string EXCHANGE_NAME = "BINANCE";
        private const string SEPARATOR = ":";
        private const string NEW_LINE = "\n";

        public string Create()
        {
            var result = new StringBuilder();
            if (analiseTrendResultViews?.Count > 0)
            {
                foreach (var analiseTrendResultView in analiseTrendResultViews)
                {
                    // записываем только тренд
                    if (analiseTrendResultView.TrendStatus.Contains(TrendStatus.DownTrend.ToString()) || analiseTrendResultView.TrendStatus.Contains(TrendStatus.Uptrend.ToString()))
                    {
                        var record = $"{EXCHANGE_NAME}{SEPARATOR}{analiseTrendResultView.Symbol}{NEW_LINE}";
                        result.Append(record);
                    }
                }
            }
            return result.ToString();
        }
    }
}
