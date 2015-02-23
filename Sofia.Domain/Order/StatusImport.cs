using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sofia.Domain.Order
{

    /// <summary>
    /// Перечисление статусов импорта в Factelligence
    /// </summary>
    public enum StatusImport
    {
        /// <summary>
        /// Не передано
        /// </summary>
        NotSentToFactelligence = 0,
        /// <summary>
        /// Передано
        /// </summary>
        SentToFactelligence = 1
    }
}
