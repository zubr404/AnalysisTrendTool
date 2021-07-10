using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BinanceAPI.MarketTime
{
    /// <summary>
    /// Получаем время биржи
    /// </summary>
    public class TimeRequester
    {
        public async Task<long> GetTime()
        {
            try
            {
                var url = UrlCreator.GetTimeUrl();
                var requester = new PublicApiRequester();
                var response = await requester.RequestPublicApi(url);
                return long.Parse(response.ResponseMessage.Replace("{\"serverTime\":", "").Replace("}", ""));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
