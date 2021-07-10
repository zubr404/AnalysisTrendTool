using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AnalisesTrendTool.Services
{
    /// <summary>
    /// Сохранят список валютных пар
    /// </summary>
    class PairsWriter
    {
        public void PairWrite(string path, string pairsList)
        {
            try
            {
                File.WriteAllText(path, pairsList, Encoding.Default);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
