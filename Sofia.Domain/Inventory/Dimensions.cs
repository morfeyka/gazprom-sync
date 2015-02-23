using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sofia.Domain.Inventory
{
    /// <summary>
    /// Предоставляет информации о размерах инвентарной позиции
    /// </summary>
    /// <remarks>table InventDim</remarks>
    public class Dimensions:EntityWithTypedId<string>
    {
        /// <summary>
        /// Возвращает или задает идентификатор области данных
        /// </summary>
        /// <example>sof</example>
        /// <remarks>field DataAreaId</remarks>
        public virtual string DataAreaId { get; set; }

        /// <summary>
        /// Возвращает или задает ширину инвентарной позиции
        /// </summary>
        /// <remarks>field SF_WidthId</remarks>
        public virtual string SF_WidthId { get; set; }
        /// <summary>
        /// Возвращает или задает цвет инвентарной позиции
        /// </summary>
        /// <remarks>field SF_ColorId</remarks>
        public virtual string SF_ColorId { get; set; }
        /// <summary>
        /// Возвращает или задает высоту инвентарной позиции
        /// </summary>
        /// <remarks>field SF_HeightId</remarks>
        public virtual string SF_HeightId { get; set; }
    }
}
