using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace zTestAnanlysesTrend
{
    public class PublicRequester
    {
        // обобщенный запрос к публичным API
        public async Task<int> RequestPublicApi(string uri)
        {
            try
            {
                var reqGET = (HttpWebRequest)WebRequest.Create(uri);
                var response = (HttpWebResponse)await reqGET.GetResponseAsync();

                var headers = response.Headers;
                Console.WriteLine("{0}: {1}", headers.GetKey(4), headers[4]);
                Console.WriteLine("{0}: {1}", headers.GetKey(5), headers[5]);

                var stream = response.GetResponseStream();
                var sr = new StreamReader(stream);
                var line1 = sr.ReadToEnd();

                var status = response.StatusCode;

                return (int)status;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
