using System;
using HG.Base.NHibernate.Domain;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Type;

namespace Sofia.Mapping
{
    public abstract class EntityMapper<TEntity> : ClassMapping<TEntity> where TEntity : class, IEntityWithTypedId<int>
    {
        protected EntityMapper()
        {
            Id(x => x.Id, map =>
            {
                map.Generator(Generators.Identity);
                map.Type((IIdentifierType)TypeFactory.Basic(typeof(Int32).FullName));
            });
        }
    }
}