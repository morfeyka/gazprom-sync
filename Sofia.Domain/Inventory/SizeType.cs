using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Sofia.Domain.Inventory
{
    /// <summary>
    /// Предоставляет информацию о типе размера номенклатурной позиции
    /// </summary>
    /// <remarks>table SF_SizeTypes</remarks>
    public class SizeType : EntityWithTypedId<string>
    {
        //private readonly ISet<Item> _items;

        //public SizeType()
        //{
        //    _items = new HashSet<Item>();
        //}

        /// <summary>
        /// Возвращает или задает стандартное количество в упаковке
        /// </summary>
        public virtual double StandartPackQty { get; set; }

        /// <summary>
        /// Возвращает коллекцию номенклатурных позиций данного типа
        /// </summary>
        //public virtual ReadOnlyCollection<Item> Items
        //{
        //    get { return new ReadOnlyCollection<Item>(new List<Item>(_items)); }
        //}
    }
}
