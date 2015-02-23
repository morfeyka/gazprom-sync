using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sofia.Domain.Inventory
{
    /// <summary>
    /// Предоставляет описание номенклатурной позиции
    /// </summary>
    /// <remarks>table InventTxt</remarks>
    public class Notes
    {
        /// <summary>
        /// Возвращает или задает описание
        /// </summary>
        /// <remarks>field txt</remarks>
        public string Name { get; set; }
        /// <summary>
        /// Возвращает или задает культуру описания <example>RU</example>
        /// </summary>
        /// <remarks>field LanguageID</remarks>
        public string Culture { get; set; }
        /// <summary>
        /// Возвращает или задает идентификатор области данных
        /// </summary>
        /// <example>sof</example>
        /// <remarks>field DataAreaId</remarks>
        public string DataAreaId { get; set; }
    }
}
