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
    public class ProductionRouteMap:ClassMapping<ProductionRoute>
    {
        public ProductionRouteMap()
        {
            Mutable(false);
            Table("ProdRoute");
            Id(x => x.Id, map =>
            {
                map.Column("RECID");
                map.Generator(Generators.Assigned);
                map.Type((IIdentifierType)TypeFactory.Basic(typeof(long).FullName));
            });

            ManyToOne(x => x.Production, map =>
            {
                map.Access(Accessor.Property);
                map.Column("PRODID");
                map.NotNullable(false);
                map.Lazy(LazyRelation.Proxy);
                map.Cascade(Cascade.None);
                map.Class(typeof(Production));
            });

            Property(x => x.OprNum, map =>
                                      {
                                          map.Column("OPRNUM");
                                          map.NotNullable(true);
                                          map.Type(TypeFactory.Basic(typeof(int).FullName));
                                      });
            Property(x => x.OprNumNext, map =>
            {
                map.Column("OPRNUMNEXT");
                map.NotNullable(true);
                map.Type(TypeFactory.Basic(typeof(int).FullName));
            });
            Property(x => x.Oprid, map =>
            {
                map.Column("OPRID");
                map.Length(10);
                map.NotNullable(true);
                map.Type(TypeFactory.Basic(typeof(string).FullName));
            });
            Property(x => x.fromdate, map =>
            {
                map.Column("FROMDATE");
                map.NotNullable(true);
                map.Type(TypeFactory.Basic(typeof(DateTime).FullName));
            });
            Property(x => x.Todate, map =>
            {
                map.Column("TODATE");
                map.NotNullable(true);
                map.Type(TypeFactory.Basic(typeof(DateTime).FullName));
            });

            Property(x => x.WrkCtrID, map =>
            {
                map.Column("WRKCTRID");
                map.Length(10);
                map.NotNullable(true);
                map.Type(TypeFactory.Basic(typeof(string).FullName));
            });

            Property(x => x.SF_PRIOR, map =>
            {
                map.Column("SF_PRIOR");
                map.NotNullable(true);
                map.Type(TypeFactory.Basic(typeof(int).FullName));
            });
            Property(x => x.Calcqty, map =>
            {
                map.Column("CALCQTY");
                map.NotNullable(true);
                map.Type(TypeFactory.Basic(typeof(double).FullName));
            });
            Property(x => x.Processperqty, map =>
            {
                map.Column("PROCESSPERQTY");
                map.NotNullable(true);
                map.Type(TypeFactory.Basic(typeof(double).FullName));
            });
            Property(x => x.Setuptime, map =>
            {
                map.Column("SETUPTIME");
                map.NotNullable(true);
                map.Type(TypeFactory.Basic(typeof(double).FullName));
            });
            Property(x => x.Processtime, map =>
            {
                map.Column("PROCESSTIME");
                map.NotNullable(true);
                map.Type(TypeFactory.Basic(typeof(double).FullName));
            });
            Property(x => x.Transptime, map =>
            {
                map.Column("TRANSPTIME");
                map.NotNullable(true);
                map.Type(TypeFactory.Basic(typeof(double).FullName));
            }); 
        }
    }
}
