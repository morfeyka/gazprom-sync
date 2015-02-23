using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Sofia.Domain.Order
{
    /// <summary>
    /// Предоставляет информацию о маршруте
    /// </summary>
    /// <remarks>table SF_WO2Ent</remarks>
    public class Route:EntityWithTypedId<long>
    {
        /// <summary>
        /// Возвращает или задает идентификатор маршрута
        /// </summary>
        /// <remarks>field RouteId</remarks>
        public virtual string RouteId { get; set; }

        /// <summary>
        /// Возвращает или задает область выбираемых данных
        /// </summary>
        /// <remarks>field DataAreaId</remarks>
        public virtual string DataAreaId { get; set; }
        /// <summary>
        /// Возвращает или задает тип открытого пути
        /// </summary>
        /// <remarks>field OW</remarks>
        public virtual string OW { get; set; }
        /// <summary>
        /// Возвращает или задает 
        /// </summary>
        /// <remarks>field TS</remarks>
        public virtual string TS { get; set; }
        /// <summary>
        /// Возвращает или задает 
        /// </summary>
        /// <remarks>field HS</remarks>
        public virtual string HS { get; set; }
        /// <summary>
        /// Возвращает или задает 
        /// </summary>
        /// <remarks>field BS</remarks>
        public virtual string BS { get; set; }
        /// <summary>
        /// Возвращает или задает 
        /// </summary>
        /// <remarks>field LS</remarks>
        public virtual string LS { get; set; }
        /// <summary>
        /// Возвращает или задает 
        /// </summary>
        /// <remarks>field EM</remarks>
        public virtual string EM { get; set; }
        /// <summary>
        /// Возвращает или задает 
        /// </summary>
        /// <remarks>field SM</remarks>
        public virtual string SM { get; set; }
        /// <summary>
        /// Возвращает или задает 
        /// </summary>
        /// <remarks>field WG</remarks>
        public virtual double WG { get; set; }
        /// <summary>
        /// Возвращает или задает 
        /// </summary>
        /// <remarks>field ST</remarks>
        public virtual string ST { get; set; }
        /// <summary>
        /// Возвращает или задает 
        /// </summary>
        /// <remarks>field TN</remarks>
        public virtual double TN { get; set; }
        /// <summary>
        /// Возвращает или задает 
        /// </summary>
        /// <remarks>field OM</remarks>
        public virtual string OM { get; set; }

        public Route()
        {
            _production = new HashSet<Production>();
        }
        private readonly ISet<Production> _production;
        /// <summary>
        /// Возвращает коллекцию номенклатурных позиций данной группы
        /// </summary>
        public virtual ReadOnlyCollection<Production> Productions
        {
            get { return new ReadOnlyCollection<Production>(new List<Production>(_production)); }
        }
        /// <summary>
        /// Возращает или задает номер операции
        /// </summary>
        /// <remarks>field OprNum</remarks>
        public virtual int OperationNumber { get; set; }

        public virtual string SN { get; set; }

        public virtual int FA { get; set; }

        public virtual int RA { get; set; }

        public virtual string SOR { get; set; }

        public virtual string BomId { get; set; }

        public virtual int HEIGHT { get; set; }

        public virtual double QTY { get; set; }
    }
}
