using System;
using System.Collections.Generic;

namespace HG.Base.NHibernate.Collections
{
    [Serializable]
    public class GenericSortedSetType<T> : GenericSetType<T>
    {
        private readonly IComparer<T> _comparer;

        public GenericSortedSetType(string role, string propertyRef, IComparer<T> comparer)
            : base(role, propertyRef)
        {
            _comparer = comparer;
        }

        public IComparer<T> Comparer
        {
            get { return _comparer; }
        }

        public override object Instantiate(int anticipatedSize)
        {
            return new SortedSet<T>(_comparer);
        }
    }
}