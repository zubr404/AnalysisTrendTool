using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarketStatistics.Trend
{
    /// <summary>
    /// Анализ тренда на основе EMA
    /// </summary>
    public class EMAAnaliseTrend
    {
        private readonly IReadOnlyCollection<EMAList> EmaDatas;
        private readonly int countHistoryValues;

        public EMAAnaliseTrend(IReadOnlyCollection<EMAList> EmaDatas, int countHistoryValues)
        {
            this.EmaDatas = EmaDatas;
            this.countHistoryValues = countHistoryValues;
            TrendStatus = TrendStatus.NoData;
            CountPeriodTrendStatus = 0;
        }

        public TrendStatus TrendStatus { get; private set; }
        public int CountPeriodTrendStatus { get; private set; }

        public void TrendAnalize()
        {
            if (checkAmountData())
            {
                var emaDatasSort = EmaDatas.OrderBy(x => x.SmoothingInterval); // сортируем по периоду усреднения ЕМА (8ЕМА, 21ЕМА, 50ЕМА ...)
                var sigleBarTrendStatuses = new List<int?>();

                // !!! БЕСКОНЕЧНЫЙ ЦИКЛ !!!
                var isBreak = false;
                for (int i = 0; true; i++) // значения ЕМА выбираем с конца коллекции
                {
                    if(isBreak) { break; }
                    var emaValues = new List<decimal>(); // сюда соберем ЕМА с разними периодами усреднения по одному бару для сравнения
                    foreach (var emaData in emaDatasSort)
                    {
                        var lastIndex = emaData.EmaValues.Count - 1;
                        var index = lastIndex - i;
                        if(index >= 0)
                        {
                            emaValues.Add(emaData.EmaValues.ElementAt(index));
                        }
                        else
                        {
                            emaValues.Add(0);
                            isBreak = true;
                        }
                    }
                    sigleBarTrendStatuses.Add(trendIdentificationSingleBar(emaValues));
                }
                trendIdentification(sigleBarTrendStatuses);
            }
        }

        // идентификация тренда на одном баре. Сравниваем значения в порядке (8ЕМА, 21ЕМА, 50ЕМА ...)
        // возвращает 1 - восходящий; 0 -нет; -1 - низходящий; null - нет данных;
        private int? trendIdentificationSingleBar(IEnumerable<decimal> emaValues)
        {
            if(emaValues?.Count() > 0)
            {
                var trendIdentificator = new TrendIdentificatorSingleBar();
                decimal? emaValuePrevios = null;
                foreach (var emaValue in emaValues)
                {
                    if (emaValue > 0)
                    {
                        if (emaValuePrevios.HasValue)
                        {
                            trendIdentificator.Identification(emaValuePrevios.Value - emaValue);
                        }
                        emaValuePrevios = emaValue;
                    }
                    else
                    {
                        return null;
                    }
                }
                return trendIdentificator.TrendStatus;
            }
            return null;
        }

        private void trendIdentification(List<int?> sigleBarTrendStatuses)
        {
            var tendIdentificator = new TrendIdentificator(sigleBarTrendStatuses);
            TrendStatus = tendIdentificator.Identification(countHistoryValues, out int countTrendStatus);
            CountPeriodTrendStatus = countTrendStatus;
        }

        private bool checkAmountData()
        {
            if (EmaDatas?.Count >= 0)
            {
                foreach (var emaData in EmaDatas)
                {
                    if (emaData.EmaValues.Count < countHistoryValues)
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
            return true;
        }
    }
}
