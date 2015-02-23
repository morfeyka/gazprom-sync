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
    public class SizeTypeMap : ClassMapping<SizeType>
    {
        public SizeTypeMap()
        {
            Mutable(false);
            Table("SF_SIZETYPES");
            Id(x=>x.Id, map=>
                            {
                                map.Column("SIZETYPEID");
                                map.Generator(Generators.Assigned);
                                map.Length(20);
                                map.Type((IIdentifierType)TypeFactory.Basic(typeof(string).FullName));
                            });
            Property(x=>x.StandartPackQty, map=>
                                               {
                                                   map.Column("STANDARTPACKQTY");
                                                   map.NotNullable(true);
                                                   map.Type(TypeFactory.Basic(typeof(double).FullName));
                                               });
        }
    }
}
