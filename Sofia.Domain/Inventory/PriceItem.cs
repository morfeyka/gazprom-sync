using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sofia.Domain.Inventory
{
    /// <summary>
    /// Предоставляет информацию о цене номенклатурной позиции <see cref="Item"/>
    /// </summary>
    /// <remarks>table InventTableModule</remarks>
    public class PriceItem:EntityWithTypedId<long>
    {
        /// <summary>
        /// Возвращает или задает цену 
        /// </summary>
        /// <remarks>field price</remarks>
        public virtual double Price { get; set; }

        /// <summary>
        /// Возвращаете или задает коэффициент
        /// </summary>
        /// <remarks>field priceUnit</remarks>
        public virtual double Unit { get; set; }

        /// <summary>
        /// <see cref="Inventory.Item"/>
        /// </summary>
        /// <remarks>field ItemID</remarks>
        public virtual Item Item { get; set; }

        /// <summary>
        /// Возвращает или задает тип цены номенклатуры
        /// </summary>
        /// <remarks>field ModuleType</remarks>
        public virtual TypePrice TypePrice { get; set; }
    }
}
