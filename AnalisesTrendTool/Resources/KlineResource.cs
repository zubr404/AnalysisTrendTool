using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace AnalisesTrendTool.Resources
{
    /// <summary>
    /// Таймфреймы свечей
    /// </summary>
    class KlineResource
    {
        public static IReadOnlyCollection<string> TimeframesAll = new ReadOnlyCollection<string>(new List<string>()
        {
            "5m", "15m", "1h", "6h", "1d", "1w"
        });
        public static IReadOnlyCollection<string> TimeframesIntraday = new ReadOnlyCollection<string>(new List<string>()
        {
            "5m", "15m", "1h", "6h"
        });

        public const int LIMIT_KLINES = 1000;
        public const int TIMEOUT_REQUEST_MS = 1000;
        public const int COUNT_REQUEST_FOR_DELAY = 19;
        public const string BINANCE = "BINANCE";
    }
}
