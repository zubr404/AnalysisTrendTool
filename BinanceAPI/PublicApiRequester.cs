using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BinanceAPI
{
    /// <summary>
    /// Запросы к публичному API
    /// </summary>
    class PublicApiRequester
    {
        public async Task<PublicAPIResponse> RequestPublicApi(string uri)
        {
            try
            {
                var result = new PublicAPIResponse();
                var reqGET = (HttpWebRequest)WebRequest.Create(uri);
                var response = (HttpWebResponse)await reqGET.GetResponseAsync();
                var stream = response.GetResponseStream();
                using (var sr = new StreamReader(stream))
                {
                    result.ResponseMessage = sr.ReadToEnd();
                }
                result.StatusCode = (int)response.StatusCode;
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
