using System;
using System.Collections.Generic;
using System.Text;

namespace BinanceAPI.KlineData
{
    public class Klines : List<Kline>
    {
        public string Simbol { get; private set; }
        public string Interval { get; private set; }
        public Klines(string simbol, string interval)
        {
            Simbol = simbol;
            Interval = interval;
        }
    }
}
