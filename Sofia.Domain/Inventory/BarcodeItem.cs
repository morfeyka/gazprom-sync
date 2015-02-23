using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sofia.Domain.Inventory
{
    /// <summary>
    /// Представляет баркод номенклатурной позиции
    /// </summary>
    /// <remarks>table InventItemBarcode</remarks>
    public class BarcodeItem
    {
        /// <summary>
        /// Возвращает или задает тип баркода <example>EAN13-сбст, RFID, EAN13</example>
        /// </summary>
        /// <remarks>field barcodeSetupId</remarks>
        public virtual string Type { get; set; }
        /// <summary>
        /// Возвращает или задает позицию <see cref="Inventory.Item"/>
        /// </summary>
        /// <remarks>field itemId</remarks>
        public virtual Item Item { get; set; }
        /// <summary>
        /// Возвращает или задает количество
        /// </summary>
        /// <remarks>field qty</remarks>
        public virtual decimal Quantity { get; set; }

        /// <summary>
        /// Возвращает или задает баркод
        /// </summary>
        /// <remarks>field itembarcode</remarks>
        public virtual string Barcode { get; set; }
    }
}
