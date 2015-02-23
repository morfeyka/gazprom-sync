using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using Sofia.Domain.Interface;
using Sofia.Domain.Order;

namespace Sofia.Mapping.Interface
{
    public class ProductContainerMap: SubclassMapping<ProductContainer>
    {
        public ProductContainerMap()
        {
            DiscriminatorValue((int) TypeData.Product);

            ManyToOne(x => x.Production, map =>
            {
                map.Access(Accessor.Property);
                map.PropertyRef("RecId");
                map.Cascade(Cascade.None);
                map.Class(typeof(Production));
                map.Column("REFRECID");
                map.Lazy(LazyRelation.NoLazy);
                map.NotNullable(false);
                map.NotFound(NotFoundMode.Ignore);
            });
        }
    }
}
