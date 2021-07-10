using AnalisesTrendTool.Services;
using AnalisesTrendTool.ViewModels;
using MarketStatistics;
using MarketStatistics.EMA;
using MarketStatistics.Trend;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using AnalisesTrendTool.zTestNamespace;

namespace AnalisesTrendTool
{
    public class ModelView
    {
        public ModelView()
        {
            SettingsScrinView = new SettingsScrinView();
            MainScrinView = new MainScrinView(SettingsScrinView);
        }

        public MainScrinView MainScrinView { get; set; }
        public SettingsScrinView SettingsScrinView { get; set; }

        public RelayCommand SettingsShowCommand
        {
            get
            {
                return settingsShowCommand ?? new RelayCommand((object o) =>
                {
                    SettingsScrinView.Visibility = System.Windows.Visibility.Visible;
                    SettingsScrinView.LoadSettings();
                });
            }
        }
        private RelayCommand settingsShowCommand;


        #region TEST
        public TestItemsList TestItemsList { get; set; }
        private void testItemsLoad()
        {
            TestItemsList = new TestItemsList();
            for (int i = 0; i < 4; i++)
            {
                var testItems = new TestItems();
                for (int ii = 0; ii < 9; ii++)
                {
                    testItems.Add(new TestItemModel()
                    {
                        Value1 = i.ToString(),
                        Value2 = ii.ToString()
                    });
                }
                TestItemsList.Add(testItems);
            }
        }



        public RelayCommand TestCommand
        {
            get
            {
                return testCommand ?? new RelayCommand(async (object o) =>
                {
                    var klineResults = new List<string>(); // для записи информации в файл
                    klineResults.Add(DateTime.Now.ToString());

                    var receiverPairs = new ReceiverPairs(@"C:\Users\sss63\All projects\Net\Upwork\Crypto price data analysis tool - Binance.com API\2_CRYPTO - ALL USDT PAIRS (2).txt", ":");
                    var receiverAnalytics = new ReceiverMarketStatistcs();
                    await receiverAnalytics.GetStatistcs(receiverPairs, false);

                    var analiseTrendResults = new List<AnaliseTrendResultView>();
                    foreach (var klines in receiverAnalytics.KlinesList)
                    {
                        var closes = new ReadOnlyCollection<decimal>(klines.Select(x => x.Close).ToList());
                        var emaCalculator1 = new EMACalculator(closes, 8);
                        emaCalculator1.EMAValuesCalculating();
                        var emaCalculator2 = new EMACalculator(closes, 21);
                        emaCalculator2.EMAValuesCalculating();
                        var emaCalculator3 = new EMACalculator(closes, 50);
                        emaCalculator3.EMAValuesCalculating();

                        var emaDatas = new ReadOnlyCollection<EMAList>(new List<EMAList>()
                        {
                            emaCalculator1.EMAList,
                            emaCalculator2.EMAList,
                            emaCalculator3.EMAList
                        });

                        // write
                        //var emaStr1 = $"{emaCalculator1.EMAList.SmoothingInterval}";
                        //foreach (var item in emaCalculator1.EMAList.EmaValues)
                        //{
                        //    emaStr1 += $" {Math.Round(item, 2)}";
                        //}
                        //var emaStr2 = $"{emaCalculator2.EMAList.SmoothingInterval} ";
                        //foreach (var item in emaCalculator2.EMAList.EmaValues)
                        //{
                        //    emaStr2 += $" {Math.Round(item, 2)}";
                        //}
                        //klineResults.Add(emaStr1);
                        //klineResults.Add(emaStr2);
                        //---------------

                        var emaAnaliseTrend = new EMAAnaliseTrend(emaDatas, 5);
                        emaAnaliseTrend.TrendAnalize();

                        analiseTrendResults.Add(new AnaliseTrendResultView()
                        {
                            Symbol = klines.Simbol,
                            Intreval = klines.Interval,
                            TrendStatus = $"{emaAnaliseTrend.TrendStatus} {emaAnaliseTrend.CountPeriodTrendStatus}"
                        });
                    }


                    //MainScrinView.AnaliseTrendResults = analiseTrendResults;



                    // запись инфы в файл с результатми анализа тренда
                    foreach (var analiseTrendResult in analiseTrendResults)
                    {
                        klineResults.Add($"{analiseTrendResult.Symbol} {analiseTrendResult.Intreval} {analiseTrendResult.TrendStatus}");
                    }
                    //--------------

                    // запись инфы в файл по свечам
                    //klineResults.Add(receiverAnalytics.KlinesList.Count.ToString());
                    //klineResults.Add(receiverAnalytics.KlinesList.GroupBy(x => x.Simbol).Count().ToString());
                    //foreach (var klines in receiverAnalytics.KlinesList)
                    //{
                    //    foreach (var kline in klines)
                    //    {
                    //        var klineStr = $"{klines.Simbol}:{klines.Interval} {kline.TimeOpen}";
                    //        klineResults.Add(klineStr);
                    //    }
                    //}
                    //--------------

                    klineResults.Add(DateTime.Now.ToString());
                    File.WriteAllLines(@"C:\tmp\klines_result.txt", klineResults.GetRange(0, klineResults.Count), Encoding.Default);
                    //-------------------------------
                });
            }
        }
        private RelayCommand testCommand;
        #endregion
    }
}
