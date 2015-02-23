using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Type;
using Sofia.Domain.Order;

namespace Sofia.Mapping.Order
{
    public class SaleMap:ClassMapping<Sale>
    {
        public SaleMap()
        {
            Mutable(false);
            Table("SALESTABLE");
            Id(x => x.Id, map =>
            {
                map.Column("SALESID");
                map.Generator(Generators.Assigned);
                map.Type((IIdentifierType)TypeFactory.Basic(typeof(string).FullName));
            });

            ManyToOne(x => x.Production, map =>
            {
                map.Column("SALESID");
                map.NotNullable(false);
                map.Lazy(LazyRelation.Proxy);
                map.Cascade(Cascade.None);
                map.Class(typeof(Production));
            });
            Property(x=>x.Name, map=>
                                    {
                                        map.Column("SALESNAME");
                                        map.Length(140);
                                        map.NotNullable(true);
                                        map.Access(Accessor.ReadOnly);
                                    });
            Property(x => x.CustAccount, map =>
            {
                map.Column("CUSTACCOUNT");
                map.Length(20);
                map.NotNullable(true);
                map.Access(Accessor.ReadOnly);
            });
        }
    }
}
