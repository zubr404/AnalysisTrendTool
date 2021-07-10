using AnalisesTrendTool.Interfaces;
using AnalisesTrendTool.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AnalisesTrendTool.Services
{
    class ReceiverPairs : IReceiverPair
    {
        private readonly string path;
        private readonly string separator;

        public ReceiverPairs(string path, string separator)
        {
            this.path = path;
            this.separator = separator;
        }

        public List<string> GetPairs()
        {
            try
            {
                var pairs = new List<string>();
                using (var reader = new StreamReader(path))
                {
                    string line;
                    while (null != (line = reader.ReadLine()))
                    {
                        var pair = getPair(line);
                        if (!string.IsNullOrWhiteSpace(pair))
                        {
                            pairs.Add(pair);
                        }
                    }
                }
                return pairs;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string getPair(string line)
        {
            var datas = line.Trim().Split(separator);
            if(datas.Length > 1)
            {
                if(datas[0] == KlineResource.BINANCE)
                {
                    return datas[1];
                }
            }
            return "";
        }
    }
}
