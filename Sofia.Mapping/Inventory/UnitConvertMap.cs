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
    public class UnitConvertMap:ClassMapping<UnitConvert>
    {
        public UnitConvertMap()
        {
            Mutable(false);
            Table("UNITCONVERT");
            Id(x => x.Id, map =>
            {
                map.Column("RECID");
                map.Generator(Generators.Assigned);
                map.Type((IIdentifierType)TypeFactory.Basic(typeof(long).FullName));
            });
            Property(x=>x.FromUnit, map=>
                                        {
                                            map.Column("FROMUNIT");
                                            map.NotNullable(true);
                                            map.Length(10);
                                        });
            Property(x => x.ToUnit, map =>
            {
                map.Column("TOUNIT");
                map.NotNullable(true);
                map.Length(10);
            });
           Property(x => x.Factor, map =>
            {
                map.Column("FACTOR");
                map.NotNullable(true);
            });
           ManyToOne(x => x.Item, map =>
           {
               map.Cascade(Cascade.None);
               map.Class(typeof(Item));
               map.Column("ITEMID");
               map.Lazy(LazyRelation.Proxy);
               map.NotNullable(false);
           });
        }
    }
}
