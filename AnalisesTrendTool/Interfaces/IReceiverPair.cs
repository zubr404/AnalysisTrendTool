using System;
using System.Collections.Generic;
using System.Text;

namespace AnalisesTrendTool.Interfaces
{
    /// <summary>
    /// Получение валютных пар
    /// </summary>
    interface IReceiverPair
    {
        List<string> GetPairs();
    }
}
