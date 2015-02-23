using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Type;
using Sofia.Domain.Inventory;
using Sofia.Domain.Order;

namespace Sofia.Mapping.Order
{
    public class ProductionBomMap:ClassMapping<ProductionBOM>
    {
        public ProductionBomMap()
        {
            Mutable(false);
            Table("PRODBOM");
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
                map.NotFound(NotFoundMode.Ignore);
                map.NotNullable(false);
            });
            ManyToOne(x => x.Unit, map =>
            {
                map.Cascade(Cascade.None);
                map.Class(typeof(Unit));
                map.Column("UNITID");
                map.Lazy(LazyRelation.Proxy);
                map.NotNullable(false);
                map.NotFound(NotFoundMode.Ignore);
            });
            ManyToOne(x => x.Item, map =>
            {
                map.Cascade(Cascade.None);
                map.Class(typeof(Item));
                map.Column("ITEMID");
                map.Lazy(LazyRelation.Proxy);
                map.NotNullable(false);
                map.NotFound(NotFoundMode.Ignore);
            });
            ManyToOne(x => x.Dimensions, map =>
            {
                map.Cascade(Cascade.None);
                map.Class(typeof(Dimensions));
                map.Column("INVENTDIMID");
                map.Lazy(LazyRelation.Proxy);
                map.NotNullable(false);
                map.NotFound(NotFoundMode.Ignore);
            });
            Property(x => x.BomId, map =>
            {
                map.Column("BOMID");
                map.Length(40);
                map.NotNullable(true);
                map.Type(TypeFactory.Basic(typeof(string).FullName));
            });
            Property(x => x.BOMConsump, map =>
            {
                map.Column("BOMCONSUMP");
                map.NotNullable(true);
                map.Type(TypeFactory.Basic(typeof(int).FullName));
            });
            Property(x => x.BOMQty, map =>
            {
                map.Column("BOMQTY");
                map.NotNullable(true);
                map.Type(TypeFactory.Basic(typeof(double).FullName));
            });

            Property(x => x.LineNum, map =>
            {
                map.Column("LINENUM");
                map.NotNullable(true);
                map.Type(TypeFactory.Basic(typeof(double).FullName));
            });
            Property(x => x.OprNum, map =>
            {
                map.Column("OPRNUM");
                map.NotNullable(true);
                map.Type(TypeFactory.Basic(typeof(int).FullName));
            });
            Property(x => x.Position, map =>
            {
                map.Column("POSITION");
                map.Length(30);
                map.NotNullable(true);
                map.Type(TypeFactory.Basic(typeof(string).FullName));
            });
            Property(x => x.ProdLineType, map =>
            {
                map.Column("PRODLINETYPE");
                map.NotNullable(true);
                map.Type(TypeFactory.Basic(typeof(int).FullName));
            });
            Property(x => x.QtyBOMCalc, map =>
            {
                map.Column("QTYBOMCALC");
                map.NotNullable(true);
                map.Type(TypeFactory.Basic(typeof(double).FullName));
            });
        }
    }
}
