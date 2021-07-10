using BinanceAPI.KlineData;
using MarketStatistics.EMA;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace zTestAnanlysesTrend
{
    class Program
    {
        private const string URL = @"https://api3.binance.com/api/v3/klines?symbol=BTCUSDT&interval=5m&limit=1";

        static void Main(string[] args)
        {
            var prices = new List<decimal>();
            for (int i = 1; i <= 100; i++)
            {
                prices.Add(i);
            }
            var emaCalculator = new EMACalculator(new ReadOnlyCollection<decimal>(prices), 3);
            emaCalculator.EMAValuesCalculating();

            Console.WriteLine(prices.Count);
            Console.WriteLine(emaCalculator.EMAList.EmaValues);
            foreach (var item in emaCalculator.EMAList.EmaValues)
            {
                Console.WriteLine(item);
            }
            

            Console.WriteLine("FINISH");
            Console.Read();
        }

        static void Test1()
        {
            var tasks = new List<Task<int>>();
            var requester = new PublicRequester();

            for (int i = 1; i <= 100; i++)
            {
                if (i % 10 == 0)
                {
                    Thread.Sleep(5000);
                }
                tasks.Add(requester.RequestPublicApi(URL));
            }
            Task.WhenAll(tasks);
            //foreach (var task in tasks)
            //{
            //    var statusCode = task.Result;
            //    if (statusCode != 200)
            //    {
            //        Console.WriteLine($"-----------> {statusCode}");
            //        break;
            //    }
            //    Console.WriteLine($"{statusCode}");
            //}
        }

        static void Test2()
        {
            var tasks = new List<Task<Klines>>();
            var klineRequester = new KlineRequester();

            for (int i = 1; i <= 0; i++)
            {
                tasks.Add(klineRequester.GetKlines("BTCUSDT", "5m", 1));
            }
            Task.WhenAll(tasks);

            foreach (var task in tasks)
            {
                foreach (var kline in task.Result)
                {
                    Console.WriteLine($"{kline.TimeOpen}");
                }
            }
        }
    }
}
