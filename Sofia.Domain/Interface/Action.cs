using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sofia.Domain.Interface
{
    /// <summary>
    /// Предоставляет типы действий с импортируемыми данными <see cref="DataImported.Action"/>
    /// </summary>
    public enum Action
    {
        /// <summary>
        /// Вставка позиции
        /// </summary>
        Insert = 0,
        /// <summary>
        /// Обновление позиции
        /// </summary>
        Update = 1,
        /// <summary>
        /// Удаление позиции
        /// </summary>
        Delete = 2
    }
}
