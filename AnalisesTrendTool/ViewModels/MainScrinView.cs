using AnalisesTrendTool.Resources;
using AnalisesTrendTool.Services;
using MarketStatistics;
using MarketStatistics.EMA;
using MarketStatistics.Trend;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace AnalisesTrendTool.ViewModels
{
    public class MainScrinView : PropertyChangedBase
    {
        private readonly SettingsScrinView settingsScrinView;
        private readonly List<int> emaPeriods;
        private readonly Dispatcher dispatcher;
        private readonly MarketTimeSynchronizer marketTimeSynchronizer;
        private readonly AnaliseTrendResultViewFactory analiseTrendResultViewFactory;
        private Task calculateTask;

        private readonly PairsWriter pairsWriter;

        public MainScrinView(SettingsScrinView settingsScrinView)
        {
            this.settingsScrinView = settingsScrinView;
            emaPeriods = new List<int>();
            dispatcher = Dispatcher.CurrentDispatcher;
            marketTimeSynchronizer = new MarketTimeSynchronizer();
            marketTimeSynchronizer.ConnectEvent += MarketTimeSynchronizer_ConnectEvent;
            analiseTrendResultViewFactory = new AnaliseTrendResultViewFactory();
            calculateTask = null;

            pairsWriter = new PairsWriter();
        }

        #region Properties
        /// <summary>
        /// Показывать/скрывать только тренд
        /// </summary>
        public bool IsOnlyTrend
        {
            get { return isOnlyTrend; }
            set
            {
                isOnlyTrend = value;
                base.NotifyPropertyChanged();
            }
        }
        private bool isOnlyTrend = true;

        /// <summary>
        /// Показывать/скрывать отсутствие данных
        /// </summary>
        public bool IsNoDataHiden
        {
            get { return isNoDataHiden; }
            set
            {
                isNoDataHiden = value;
                base.NotifyPropertyChanged();
            }
        }
        private bool isNoDataHiden = true;

        /// <summary>
        /// Загружать данные только внутри дня
        /// </summary>
        public bool IsIntradayOnly
        {
            get { return isIntradayOnly; }
            set
            {
                isIntradayOnly = value;
                base.NotifyPropertyChanged();
            }
        }
        private bool isIntradayOnly = false;

        /// <summary>
        /// Результат анализа тренда
        /// </summary>
        //public List<AnaliseTrendResultView> AnaliseTrendResults
        //{
        //    get { return analiseTrendResults; }
        //    set
        //    {
        //        if(value?.Count > 0)
        //        {
        //            analiseTrendResults = value;
        //            base.NotifyPropertyChanged();
        //        }
        //    }
        //}
        //private List<AnaliseTrendResultView> analiseTrendResults;

        public List<List<AnaliseTrendResultView>> AnaliseTrendResultsList
        {
            get { return analiseTrendResultsList; }
            set
            {
                if (value?.Count > 0)
                {
                    analiseTrendResultsList = value;
                    base.NotifyPropertyChanged();
                }
            }
        }
        private List<List<AnaliseTrendResultView>> analiseTrendResultsList;

        public SolidColorBrush StateLabelForeground
        {
            get { return stateLabelForeground; }
            set
            {
                stateLabelForeground = value;
                base.NotifyPropertyChanged();
            }
        }
        private SolidColorBrush stateLabelForeground;

        public string StateLabelText
        {
            get { return stateLabelText; }
            set
            {
                stateLabelText = value;
                base.NotifyPropertyChanged();
            }
        }
        private string stateLabelText;

        public string WarningLabelText
        {
            get { return warningLabelText; }
            set
            {
                warningLabelText = value;
                base.NotifyPropertyChanged();
            }
        }
        private string warningLabelText;
        #endregion

        private async Task Calculating()
        {
            if (calculateTask != null && !calculateTask.IsCompleted)
            {
                return;
            }
            calculateTask = Task.Run(async () =>
            {
                try
                {
                    StateLabelForeground = Brushes.Red;
                    StateLabelText = "loading data...";
                    emaPeriods.Clear();
                    emaPeriods.Add(settingsScrinView.EmaPeriodSettings1);
                    emaPeriods.Add(settingsScrinView.EmaPeriodSettings2);
                    emaPeriods.Add(settingsScrinView.EmaPeriodSettings3);

                    var receiverPairs = getReceiverPairs();

                    if (receiverPairs != null)
                    {
                        var receiverAnalytics = new ReceiverMarketStatistcs();
                        await receiverAnalytics.GetStatistcs(receiverPairs, IsIntradayOnly);

                        var _analiseTrendResults = new List<AnaliseTrendResultView>(); // все данные с учетом фильтров чекбоксов
                        foreach (var klines in receiverAnalytics.KlinesList)
                        {
                            var closes = new ReadOnlyCollection<decimal>(klines.Select(x => x.Close).ToList());

                            // строим EMA
                            var emaLists = new List<EMAList>();
                            foreach (var emaPeriod in emaPeriods)
                            {
                                var emaCalculator = new EMACalculator(closes, emaPeriod);
                                emaCalculator.EMAValuesCalculating();
                                emaLists.Add(emaCalculator.EMAList);
                            }
                            var emaDatas = new ReadOnlyCollection<EMAList>(emaLists);

                            var emaAnaliseTrend = new EMAAnaliseTrend(emaDatas, settingsScrinView.NumberBarAnalysisSettings);
                            emaAnaliseTrend.TrendAnalize();

                            // фильтруем данные по чекбоксам
                            if (IsOnlyTrend) // оставляем только тренд, если стоит галочка в чекбоксе
                            {
                                if (emaAnaliseTrend.TrendStatus == TrendStatus.DownTrend || emaAnaliseTrend.TrendStatus == TrendStatus.Uptrend)
                                {
                                    _analiseTrendResults.Add(analiseTrendResultViewFactory.Create(klines, emaAnaliseTrend));
                                }
                            }
                            else if (IsNoDataHiden)
                            {
                                if (emaAnaliseTrend.TrendStatus != TrendStatus.NoData)
                                {
                                    _analiseTrendResults.Add(analiseTrendResultViewFactory.Create(klines, emaAnaliseTrend));
                                }
                            }
                            else
                            {
                                _analiseTrendResults.Add(analiseTrendResultViewFactory.Create(klines, emaAnaliseTrend));
                            }
                        }
                        var analiseTrendResultsOrdered = _analiseTrendResults.OrderBy(x => x.CountPeriodTrendStatus).ToList();

                        // записываем пары с трендом
                        var pairsListTextCreator = new PairsListTextCreator(analiseTrendResultsOrdered);
                        var pairsList = pairsListTextCreator.Create();
                        try
                        {
                            pairsWriter.PairWrite(settingsScrinView.PairsSaveFileNameSettings, pairsList);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Writing a file", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        //------------------------------

                        // распределяем данные по таймфреймам (не использую group by из-за сортировки по таймфрейму)
                        var _analiseTrendResultsList = new List<List<AnaliseTrendResultView>>();
                        foreach (var timeFrame in KlineResource.TimeframesAll)
                        {
                            var trendResult = _analiseTrendResults.Where(x => x.Intreval == timeFrame);
                            if (trendResult?.Count() > 0)
                            {
                                _analiseTrendResultsList.Add(trendResult.OrderBy(x => x.CountPeriodTrendStatus).ToList());
                            }
                        }
                        AnaliseTrendResultsList = _analiseTrendResultsList;
                        //-----------------------------------

                        StateLabelForeground = Brushes.Green;
                        StateLabelText = "data uploaded";
                        WarningLabelText = "";
                    }
                    else
                    {
                        StateLabelForeground = Brushes.Red;
                        StateLabelText = "error";
                        WarningLabelText = "";
                        return;
                    }
                    
                }
                catch (Exception)
                {
                    StateLabelForeground = Brushes.Red;
                    StateLabelText = "error";
                    WarningLabelText = "";
                }
            });
            await calculateTask;
        }

        private ReceiverPairs getReceiverPairs()
        {
            ReceiverPairs receiverPairs = null;
            try
            {
                receiverPairs = new ReceiverPairs(settingsScrinView.PairsFileNameSettings, ":");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return receiverPairs;
        }

        private async void MarketTimeSynchronizer_ConnectEvent(object sender, EventArgs e)
        {
            await dispatcher.InvokeAsync(async () =>
            {
                await Calculating();
            });
        }
    }
}
