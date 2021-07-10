using System;
using System.Collections.Generic;
using System.Text;

namespace MarketStatistics.Trend
{
    /// <summary>
    /// Определяет тренд на одном баре
    /// </summary>
    class TrendIdentificatorSingleBar
    {
        /// <summary>
        /// Состояние тренда. 1 - восходящий; 0 -нет; -1 - низходящий;
        /// </summary>
        public int? TrendStatus { get; private set; } = null;

        public void Identification(decimal statusTrend)
        {
            if (TrendStatus.HasValue)
            {
                if (Math.Sign(TrendStatus.Value) != Math.Sign(statusTrend))
                {
                    TrendStatus = 0;
                }
            }
            else
            {
                if (statusTrend > 0)
                {
                    TrendStatus = 1;
                }
                else if (statusTrend == 0)
                {
                    TrendStatus = 0;
                }
                else
                {
                    TrendStatus = -1;
                }
            }
        }
    }
}
