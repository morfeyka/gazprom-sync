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
    public class PriceItemMap:ClassMapping<PriceItem>
    {
        public PriceItemMap()
        {
            Mutable(false);
            Table("INVENTTABLEMODULE");
            Id(x => x.Id, map =>
            {
                map.Column("RECID");
                map.Generator(Generators.Assigned);
                map.Type((IIdentifierType)TypeFactory.Basic(typeof(long).FullName));
            });
            ManyToOne(x => x.Item, map =>
            {
                map.Cascade(Cascade.None);
                map.Class(typeof(Item));
                map.Column("ITEMID");
                map.Lazy(LazyRelation.Proxy);
                map.NotNullable(false);
            });
            Property(x => x.TypePrice, map =>
            {
                map.Column("MODULETYPE");
                map.NotNullable(false);
                map.Type(NHibernate.NHibernateUtil.GuessType(typeof(TypePrice)));
            });

            Property(x => x.Price, map => { map.Column("PRICE"); map.NotNullable(false); });
            Property(x => x.Unit, map => { map.Column("PRICEUNIT"); map.NotNullable(false); });
        }
    }
}
