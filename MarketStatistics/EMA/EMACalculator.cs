using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace MarketStatistics.EMA
{
    /// <summary>
    /// Расчет экспоненциальной скользящей средней
    /// </summary>
    public class EMACalculator
    {
        private readonly decimal weightingFactor;   // коеф взвешивания
        private readonly int smoothingInterval;     // интервал сглаживания
        private readonly IReadOnlyCollection<decimal> prices;

        public EMACalculator(IReadOnlyCollection<decimal> prices, int smoothingInterval)
        {
            this.smoothingInterval = smoothingInterval;
            this.prices = prices;
            weightingFactor = getWeightingFactor();
            EMAList = new EMAList(smoothingInterval);
        }

        public EMAList EMAList { get; private set; }

        public void EMAValuesCalculating()
        {
            if (prices != null)
            {
                var countPrices = prices.Count;
                if (countPrices >= smoothingInterval)
                {
                    var emaValues = new List<decimal>(countPrices - smoothingInterval + 1);
                    var emaPrevios = getSmaValue(prices.Take(smoothingInterval));
                    emaValues.Add(emaPrevios);
                    for (int i = smoothingInterval; i < countPrices; i++)
                    {
                        var price = prices.ElementAt(i);
                        var ema = getEmaValue(price, emaPrevios);
                        emaValues.Add(ema);
                        emaPrevios = ema;
                    }
                    EMAList.EmaValues = new ReadOnlyCollection<decimal>(emaValues);
                }
            }
        }

        private decimal getWeightingFactor()
        {
            if(smoothingInterval > 0)
            {
                var const1 = 1m;
                var const2 = 2m;
                return const2 / (smoothingInterval + const1);
            }
            else
            {
                throw new ArgumentException("Smoothing Interval <= 0", nameof(smoothingInterval));
            }
        }

        private decimal getEmaValue(decimal price, decimal ema)
        {
            return weightingFactor * price + (1 - weightingFactor) * ema;
        }

        private decimal getSmaValue(IEnumerable<decimal> prices)
        {
            if (prices?.Count() > 0)
            {
                return prices.Sum() / prices.Count();
            }
            return 0;
        }
    }
}
