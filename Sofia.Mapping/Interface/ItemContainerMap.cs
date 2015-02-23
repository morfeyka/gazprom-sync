using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using Sofia.Domain.Interface;
using Sofia.Domain.Inventory;

namespace Sofia.Mapping.Interface
{
    public class ItemContainerMap: SubclassMapping<ItemContainer>
    {
        public ItemContainerMap()
        {
            DiscriminatorValue((int) TypeData.ItemInventory);

            ManyToOne(x => x.Item, map =>
            {
                map.Access(Accessor.Field);
                map.PropertyRef("RecId");
                map.Cascade(Cascade.None);
                map.Class(typeof(Item));
                map.Column("REFRECID");
                map.Lazy(LazyRelation.NoLazy);
                map.NotNullable(false);
                map.NotFound(NotFoundMode.Ignore);
            });
        }
    }
}
