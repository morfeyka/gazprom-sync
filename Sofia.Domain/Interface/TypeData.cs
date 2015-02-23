using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sofia.Domain.Interface
{
    /// <summary>
    /// Предоставляет тип импортируемых даннх
    /// </summary>
    public enum TypeData
    {
        /// <summary>
        /// Номенклатура
        /// </summary>
        /// <remarks>table InventTable</remarks>
        ItemInventory = 175,
        /// <summary>
        /// Группа номенклатуры
        /// </summary>
        /// <remarks>table InventItemGroup</remarks>
        GroupInventory = 152,
        /// <summary>
        /// Продукты
        /// </summary>
        /// <remarks>table ProdTable</remarks>
        Product = 262
    }
}
