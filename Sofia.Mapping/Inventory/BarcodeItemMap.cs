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
    public class BarcodeItemMap: ClassMapping<BarcodeItem>
    {
        public BarcodeItemMap()
        {
            Mutable(false);
            Table("INVENTITEMBARCODE");
            Id(x => x.Barcode, map =>
                                   {
                                       map.Column("ITEMBARCODE");
                                       map.Length(80);
                                       map.Generator(Generators.Assigned);
                                       map.Type((IIdentifierType)TypeFactory.Basic(typeof(string).FullName));
                                   });

            ManyToOne(x => x.Item, map =>
                                       {
                                           map.Column("ITEMID");
                                           map.NotNullable(false);
                                           map.Lazy(LazyRelation.Proxy);
                                           map.Cascade(Cascade.None);
                                           map.Class(typeof(Item));
                                       });

            Property(x => x.Type, map => { map.Column("BARCODESETUPID"); map.Length(10); map.NotNullable(false);});
            Property(x => x.Quantity, map => { map.Column("QTY");
                                                 map.NotNullable(false);
                                                 map.Type(TypeFactory.Basic(typeof (decimal).FullName));
            });
            //        Property(x => x.INVENTDIMID, map => { map.Column("INVENTDIMID"); map.Length(20); map.NotNullable(false); });
            //        Property(x => x.BARCODESETUPID, map => { map.Column("BARCODESETUPID"); map.Length(10); map.NotNullable(false); });
            //        Property(x => x.USEFORPRINTING, map => { map.Column("USEFORPRINTING"); map.NotNullable(false); });
            //        Property(x => x.USEFORINPUT, map => { map.Column("USEFORINPUT"); map.NotNullable(false); });
            //        Property(x => x.DESCRIPTION, map => { map.Column("DESCRIPTION"); map.Length(60); map.NotNullable(false); });
            
            //        Property(x => x.MODIFIEDDATE, map => { map.Column("MODIFIEDDATE"); map.NotNullable(false); });
            //        Property(x => x.MODIFIEDTIME, map => { map.Column("MODIFIEDTIME"); map.NotNullable(false); });
            //        Property(x => x.MODIFIEDBY, map => { map.Column("MODIFIEDBY"); map.Length(5); map.NotNullable(false); });
            //        Property(x => x.RECVERSION, map => { map.Column("RECVERSION"); map.NotNullable(false); });
            //        Property(x => x.RECID, map => { map.Column("RECID"); map.NotNullable(false); });        }
        }
    }
}
