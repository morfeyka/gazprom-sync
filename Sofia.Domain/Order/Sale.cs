using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sofia.Domain.Order
{
    /// <summary>
    /// Предоставляет информацию о распродаже 
    /// </summary>
    /// <remarks>table SalesTable id SalesId</remarks>
    public class Sale:EntityWithTypedId<string>
    {
        /// <summary>
        /// Возвращает или задает идентификатор области данных
        /// </summary>
        /// <example>sof</example>
        /// <remarks>field DataAreaId</remarks>
        public virtual string DataAreaId { get; set; }

        /// <summary>
        /// Возвращает или задает заказанную продукцию 
        /// </summary>
        /// <remarks>field SalesId</remarks>
        public virtual Production Production { get; set; }

        /// <summary>
        /// Возвращает или задает наименование
        /// </summary>
        /// <remarks>field SalesName</remarks>
        public virtual string Name { get; set; }

        /// <summary>
        /// Возвращает или задает идентификатор аккаунта
        /// </summary>
        /// <remarks>field CustAccount</remarks>
        public virtual string CustAccount { get; set; }
    }
}
