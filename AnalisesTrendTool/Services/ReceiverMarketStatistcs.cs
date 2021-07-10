using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AnalisesTrendTool.Interfaces;
using AnalisesTrendTool.Resources;
using BinanceAPI.KlineData;

namespace AnalisesTrendTool.Services
{
    /// <summary>
    /// Получатель статистики
    /// </summary>
    class ReceiverMarketStatistcs
    {
        public List<Klines> KlinesList { get; private set; }

        public ReceiverMarketStatistcs()
        {
            KlinesList = new List<Klines>();
        }

        public async Task GetStatistcs(IReceiverPair receiverPair, bool isIntraday)
        {
            KlinesList.Clear();

            var pairs = receiverPair.GetPairs();
            if (pairs?.Count > 0)
            {
                var tasks = new List<Task<Klines>>();
                var klineRequester = new KlineRequester();

                var countRequest = 1;
                var intervals = KlineResource.TimeframesAll;
                if (isIntraday)
                {
                    intervals = KlineResource.TimeframesIntraday;
                }

                foreach (var pair in pairs)
                {
                    foreach (var interval in intervals)
                    {
                        if (countRequest % KlineResource.COUNT_REQUEST_FOR_DELAY == 0)
                        {
                            Thread.Sleep(KlineResource.TIMEOUT_REQUEST_MS);
                        }
                        tasks.Add(klineRequester.GetKlines(pair, interval, KlineResource.LIMIT_KLINES));
                        countRequest++;
                    }
                }
                await Task.WhenAll(tasks);

                foreach (var task in tasks)
                {
                    KlinesList.Add(task.Result);
                }
            }
        }
    }
}
