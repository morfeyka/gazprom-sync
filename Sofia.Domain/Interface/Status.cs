using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sofia.Domain.Interface
{
    /// <summary>
    /// Предоставляет статусы данных для импорта <see cref="DataImported"/>
    /// </summary>
    public enum Status
    {
        /// <summary>
        /// В ожидании импорта
        /// </summary>
        Pending = 0,
        /// <summary>
        /// Начался импорт
        /// </summary>
        Started = 1,
        /// <summary>
        /// В процессе импорта
        /// </summary>
        Processed = 2,
        /// <summary>
        /// Импрорт пропущен
        /// </summary>
        Skipped = 3,
        /// <summary>
        /// Ошибка импорта
        /// </summary>
        Error = 4
    }
}
