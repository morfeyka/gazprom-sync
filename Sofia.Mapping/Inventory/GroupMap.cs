using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Type;
using Sofia.Domain.Inventory;

namespace Sofia.Mapping.Inventory
{
    public class GroupMap:ClassMapping<Group>
    {
        public GroupMap()
        {
            Mutable(false);
            Table("INVENTITEMGROUP");
            Id(x => x.Id, map =>
            {
                map.Column("ITEMGROUPID");
                map.Generator(Generators.Assigned);
                map.Type((IIdentifierType)TypeFactory.Basic(typeof(string).FullName));
            });
            Property(x=>x.StandartPackQty,map=>
                                              {
                                                  map.Column("SF_STANDARTPACKQTY");
                                                  map.NotNullable(true);
                                              });
            Property(x => x.Name, map =>
            {
                map.Column("NAME");
                map.Length(140);
                map.NotNullable(true);
            });
            Property(x => x.RecId, map =>
            {
                map.Column("RECID");
                map.NotNullable(true);
            });
            Set(p => p.Items, cm =>
            {
                cm.Access(Accessor.Field);
                cm.Cascade(Cascade.None);
                cm.Fetch(CollectionFetchMode.Select);
                cm.Inverse(true);
                cm.Key(km =>
                {
                    km.Column("ITEMGROUPID");
                    km.NotNullable(false);
                });
                cm.Lazy(CollectionLazy.Lazy);
            }, m => m.OneToMany(om => om.Class(typeof(Item))));
        }
    }
}
