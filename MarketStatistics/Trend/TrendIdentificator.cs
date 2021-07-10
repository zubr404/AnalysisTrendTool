using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarketStatistics.Trend
{
    /// <summary>
    /// Определяет тренд по множеству статусов тренда еденичного бара
    /// </summary>
    class TrendIdentificator
    {
        private readonly IReadOnlyCollection<int?> singleBarTrendStatuses;
        public TrendIdentificator(IReadOnlyCollection<int?> singleBarTrendStatuses)
        {
            this.singleBarTrendStatuses = singleBarTrendStatuses;
        }

        private TrendStatus trendStatusPrevios;
        private int countStatus;

        public TrendStatus Identification(int countHistoryValues, out int countTrendStatus)
        {
            trendStatusPrevios = TrendStatus.NoData;
            var result = TrendStatus.NoData;
            countTrendStatus = 0;

            if (singleBarTrendStatuses?.Count > 0)
            {
                for (int i = 1; i < singleBarTrendStatuses.Count; i++)
                {
                    var status = singleBarTrendStatuses.ElementAt(i); // в расчет берем только закрытые быры
                    if (status.HasValue)
                    {
                        if (status.Value > 0)
                        {
                            if (trendStatusPrevios == TrendStatus.Uptrend || trendStatusPrevios == TrendStatus.NoData)
                            {
                                countStatus++;
                                if(countStatus >= countHistoryValues)
                                {
                                    countTrendStatus = countStatus - countHistoryValues;
                                    result = TrendStatus.Uptrend;
                                    trendStatusPrevios = result;
                                }
                            }
                            else
                            {
                                if (countStatus < countHistoryValues)
                                {
                                    result = TrendStatus.NoTrend;
                                    countTrendStatus = 0;
                                    return result;
                                }
                                else
                                {
                                    return result;
                                }
                            }
                        }
                        else if (status.Value == 0)
                        {
                            if (countStatus < countHistoryValues)
                            {
                                result = TrendStatus.NoTrend;
                                countTrendStatus = 0;
                                return result;
                            }
                            else
                            {
                                return result;
                            }
                        }
                        else
                        {
                            if (trendStatusPrevios == TrendStatus.DownTrend || trendStatusPrevios == TrendStatus.NoData)
                            {
                                countStatus++;
                                if (countStatus >= countHistoryValues)
                                {
                                    countTrendStatus = countStatus - countHistoryValues;
                                    result = TrendStatus.DownTrend;
                                    trendStatusPrevios = result;
                                }
                            }
                            else
                            {
                                if (countStatus < countHistoryValues)
                                {
                                    result = TrendStatus.NoTrend;
                                    countTrendStatus = 0;
                                    return result;
                                }
                                else
                                {
                                    return result;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (countStatus < countHistoryValues)
                        {
                            result = TrendStatus.NoTrend;
                            countTrendStatus = 0;
                            return result;
                        }
                    }
                }
            }
            else
            {
                result = TrendStatus.NoTrend;
                countTrendStatus = 0;
                return result;
            }
            return result;
        }
    }
}
