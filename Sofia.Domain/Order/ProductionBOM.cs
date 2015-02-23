using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sofia.Domain.Inventory;

namespace Sofia.Domain.Order
{
    /// <summary>
    /// Предоставляет информацию о таблице ProdBOM
    /// </summary>
    /// <remarks>table ProdBOM</remarks>
    public class ProductionBOM:EntityWithTypedId<long>
    {
        /// <summary>
        /// Возвращает или задает BomID
        /// </summary>
        public virtual string BomId { get; set; }

        public virtual Unit Unit { get; set; }

        //public virtual int Id { get; set; }

        /// <summary>
        /// Возвращает или задает продукцию
        /// </summary>
        /// <remarks>field ProdId</remarks>
        public virtual Production Production { get; set; }

        /// <summary>
        /// Возвращает или задает количество
        /// </summary>
        /// <remarks>field BOMQty</remarks>
        public virtual double BOMQty { get; set; }

        /// <summary>
        /// Возвращает или задает номер операции
        /// </summary>
        /// <remarks>field OprNum</remarks>
        public virtual int OprNum { get; set; }

        /// <summary>
        /// Возвращает или задает область импортируемых данных
        /// </summary>
        public virtual string DataAreaId { get; set; }

        /// <summary>
        /// Возвращает или задает номер линии
        /// </summary>
        public virtual double LineNum { get; set; }
        /// <summary>
        /// Возвращает или задает инвентарную позицию
        /// </summary>
        /// <remarks>field ItemId</remarks>
        public virtual Inventory.Item Item { get; set; }

        public virtual Dimensions Dimensions { get; set;}

        public virtual int ProdLineType { get; set; }

        public virtual string Position { get; set; }

        public virtual double QtyBOMCalc { get; set; }

        public virtual int BOMConsump { get; set; }
    }
}
