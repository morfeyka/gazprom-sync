using System.Collections.Generic;
using System.Reflection;

namespace HG.Base.NHibernate.Domain
{
    public interface IEntityWithTypedId<out TId>
    {
        TId Id { get; }
        IEnumerable<PropertyInfo> GetSignatureProperties();
        bool IsTransient();
    }
}