using System;
using System.Collections.Generic;
using System.Text;

namespace BinanceAPI.KlineData
{
    /// <summary>
    /// Модель свечи
    /// </summary>
    public class Kline
    {
        public long TimeOpen { get; }
        public decimal High { get; }
        public decimal Low { get; }
        public decimal Open { get; }
        public decimal Close { get; }

        public Kline(long timeOpen, decimal high, decimal low, decimal open, decimal close)
        {
            TimeOpen = timeOpen;
            High = high;
            Low = low;
            Open = open;
            Close = close;
        }
    }
}
