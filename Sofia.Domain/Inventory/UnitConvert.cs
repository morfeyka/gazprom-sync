using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sofia.Domain.Inventory
{
    /// <summary>
    /// Предоставляет единиц измерения для конвертации
    /// </summary>
    public class UnitConvert:EntityWithTypedId<long>
    {
        /// <summary>
        /// Возращает или задает инвентарную позицию 
        /// </summary>
        public virtual Item Item { get; set; }

        /// <summary>
        /// Возращает или задает из какой конвертировать
        /// </summary>
        public virtual string FromUnit { get; set; }

        /// <summary>
        /// Возращает или задает в какую конвертировать
        /// </summary>
        public virtual string ToUnit { get; set; }

        public virtual double Factor { get; set; }
    }
}
