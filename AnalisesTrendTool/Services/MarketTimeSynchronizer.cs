using BinanceAPI.MarketTime;
using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace AnalisesTrendTool.Services
{
    class MarketTimeSynchronizer
    {
        private long marketTimePoint; // ближайшая пятиминутка в будущем
        private const long MSC_FOR_5M = 300000; // количество миллисекунд для 5-ти минутного таймфрейма
        private readonly Timer timer;
        private readonly TimeRequester timeRequester;
        public event EventHandler ConnectEvent;

        public MarketTimeSynchronizer()
        {
            marketTimePoint = 0;
            timeRequester = new TimeRequester();
            ConnectEvent = delegate { };
            timer = new Timer(10000);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private async void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var marketTime = await timeRequester.GetTime();
            if (marketTimePoint == 0 || marketTime > marketTimePoint)
            {
                marketTimePoint = (marketTime / MSC_FOR_5M) * MSC_FOR_5M + MSC_FOR_5M;
                OnConnectEvent();
            }
        }

        // звпускаем события для старта всех расчетов
        protected virtual void OnConnectEvent()
        {
            ConnectEvent(this, new EventArgs());
        }
    }
}
