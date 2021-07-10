using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace BinanceAPI.KlineData
{
    /// <summary>
    /// Получение информации по свечам
    /// </summary>
    public class KlineRequester
    {
        public async Task<Klines> GetKlines(string simbol, string interval, int limit)
        {
            try
            {
                var result = new Klines(simbol, interval);
                var url = UrlCreator.GetKlineUrl(simbol, interval, limit);

                var requester = new PublicApiRequester();
                var response = await requester.RequestPublicApi(url);

                var klines = JConverter.JsonConver<List<object[]>>(response.ResponseMessage);
                foreach (var k in klines)
                {
                    var kline = new Kline(
                        Convert.ToInt64(k[0], new CultureInfo("en-US")),
                        Convert.ToDecimal(k[2], new CultureInfo("en-US")),
                        Convert.ToDecimal(k[3], new CultureInfo("en-US")),
                        Convert.ToDecimal(k[1], new CultureInfo("en-US")),
                        Convert.ToDecimal(k[4], new CultureInfo("en-US")));
                    result.Add(kline);
                }

                return result;
            }
            catch (Exception)
            {
                return new Klines(simbol, interval);
            }
        }
    }
}
