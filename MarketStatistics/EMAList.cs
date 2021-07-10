using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MarketStatistics
{
    /// <summary>
    /// Класс с множеством значений ЕМА
    /// </summary>
    public class EMAList
    {
        /// <summary>
        /// Интевал усреднения
        /// </summary>
        public int SmoothingInterval { get; private set; }
        /// <summary>
        /// Набор значений ЕМА
        /// </summary>
        public IReadOnlyCollection<decimal> EmaValues { get; set; }

        public EMAList(int smoothingInterval)
        {
            SmoothingInterval = smoothingInterval;
            EmaValues = new ReadOnlyCollection<decimal>(new List<decimal>(0));
        }
    }
}
