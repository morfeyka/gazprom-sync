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
    public class UnitMap:ClassMapping<Unit>
    {
        public UnitMap()
        {
            Mutable(false);
            Table("UNIT");
            Id(x=>x.Id, map=>
                            {
                                map.Column("UNITID");
                                map.Generator(Generators.Assigned);
                                map.Length(10);
                                map.Type((IIdentifierType)TypeFactory.Basic(typeof(string).FullName));
                            });
        }
    }
}
