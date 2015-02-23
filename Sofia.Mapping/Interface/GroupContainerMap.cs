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
    public class GroupContainerMap: SubclassMapping<GroupContainer>
    {
        public GroupContainerMap()
        {
            DiscriminatorValue((int) TypeData.GroupInventory);
            ManyToOne(x => x.Group, map =>
            {
                map.Access(Accessor.Property);
                map.PropertyRef("RecId");
                map.Cascade(Cascade.None);
                map.Class(typeof(Group));
                map.Column("REFRECID");
                map.Lazy(LazyRelation.NoLazy);
                map.NotNullable(false);
                map.NotFound(NotFoundMode.Ignore);
            });
        }

        

    }
}
