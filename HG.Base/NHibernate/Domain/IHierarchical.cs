using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace HG.Base.NHibernate.Domain
{
    /// <summary>
    /// Представляет набор правил для объектов, имеющих иерархическую структуру.
    /// </summary>
    public interface IHierarchical<T>
    {
        /// <summary>
        /// Возвращает родительский элемент.
        /// </summary>
        T Parent { get; }
        /// <summary>
        /// Возвращает набор из дочерних элементов.
        /// </summary>
        ReadOnlyCollection<T> Childs { get; }
    }
}