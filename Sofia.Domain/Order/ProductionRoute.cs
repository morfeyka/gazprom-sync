using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sofia.Domain.Order
{
    /// <summary>
    /// Предоставляет о маршрутах продукции
    /// </summary>
    /// <remarks>table ProdRoute</remarks>
    public class ProductionRoute:EntityWithTypedId<long>
    {
        /// <summary>
        /// Возвращает или задает продукцию 
        /// </summary>
        /// <remarks>field ProdID</remarks>
        public virtual Production Production { get; set; }

        /// <summary>
        /// Возвращает или задает область выбираемых данных
        /// </summary>
        /// <remarks>field DataAreaId</remarks>
        public virtual string DataAreaId { get; set; }

        /// <summary>
        /// Возвращает или задает порядок операций
        /// </summary>
        /// <remarks>field Oprnum</remarks>
        public virtual int OprNum { get; set; }

        /// <summary>
        /// Возвращает или задает 
        /// </summary>
        /// <remarks>field OprNumNext</remarks>
        public virtual int OprNumNext { get; set; }
        /// <summary>
        /// Возвращает или задает 
        /// </summary>
        /// <remarks>field fromdate</remarks>
        public virtual DateTime fromdate { get; set; }
        /// <summary>
        /// Возвращает или задает 
        /// </summary>
        /// <remarks>field Todate</remarks>
        public virtual DateTime Todate { get; set; }

        /// <summary>
        /// Возвращает или задает 
        /// </summary>
        /// <remarks>field WrkCtrID</remarks>
        public virtual string WrkCtrID { get; set; }

        /// <summary>
        /// Возвращает или задает 
        /// </summary>
        /// <remarks>field Oprid</remarks>
        public virtual string Oprid { get; set; }

        /// <summary>
        /// Возвращает или задает 
        /// </summary>
        /// <remarks>field SF_PRIOR</remarks>
        public virtual int SF_PRIOR {get; set; }

        /// <summary>
        /// Возвращает или задает Calcqty
        /// </summary>
        /// <remarks>field Calcqty</remarks>
        public virtual double Calcqty { get; set ;}
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>field Processperqty</remarks>
        public virtual double Processperqty {get; set;}

        public virtual double Setuptime { get; set; }

        public virtual double Processtime { get; set; }

        public virtual double Transptime { get; set; }
    }
}
