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
    public class RouteMap:ClassMapping<Route>
    {
        public RouteMap()
        {
            Mutable(false);
            Table("SF_WO2ENT");
            Id(x => x.Id, map =>
            {
                map.Column("RECID");
                map.Generator(Generators.Assigned);
                map.Type((IIdentifierType)TypeFactory.Basic(typeof(long).FullName));
            });

            Id(x => x.Id, map =>
            {
                map.Column("RECID");
                map.Generator(Generators.Assigned);
                map.Type((IIdentifierType)TypeFactory.Basic(typeof(long).FullName));
            });
            Property(x => x.OperationNumber, map =>
            {
                map.Column("OPRNUM");
                map.NotNullable(true);
                map.Type(TypeFactory.Basic(typeof(int).FullName));
            });
            Property(x => x.BS, map =>
            {
                map.Column("BS");
                map.NotNullable(true);
                map.Length(20);
                map.Type(TypeFactory.Basic(typeof(string).FullName));
            });
            Property(x => x.BomId, map =>
            {
                map.Column("BOMID");
                map.NotNullable(true);
                map.Length(40);
                map.Type(TypeFactory.Basic(typeof(string).FullName));
            });
            Property(x => x.EM, map =>
            {
                map.Column("EM");
                map.NotNullable(true);
                map.Length(20);
                map.Type(TypeFactory.Basic(typeof(string).FullName));
            });
            Property(x => x.FA, map =>
            {
                map.Column("FA");
                map.NotNullable(true);
                map.Type(TypeFactory.Basic(typeof(int).FullName));
            });
            Property(x => x.HEIGHT, map =>
            {
                map.Column("HEIGHT");
                map.NotNullable(true);
                map.Type(TypeFactory.Basic(typeof(int).FullName));
            });
            Property(x => x.HS, map =>
            {
                map.Column("HS");
                map.NotNullable(true);
                map.Length(20);
                map.Type(TypeFactory.Basic(typeof(string).FullName));
            });
            Property(x => x.LS, map =>
            {
                map.Column("LS");
                map.NotNullable(true);
                map.Length(20);
                map.Type(TypeFactory.Basic(typeof(string).FullName));
            });
            Property(x => x.OM, map =>
            {
                map.Column("OM");
                map.NotNullable(true);
                map.Length(1);
                map.Type(TypeFactory.Basic(typeof(string).FullName));
            });
            Property(x => x.OW, map =>
            {
                map.Column("OW");
                map.NotNullable(true);
                map.Length(1);
                map.Type(TypeFactory.Basic(typeof(string).FullName));
            });
            Property(x => x.QTY, map =>
            {
                map.Column("QTY");
                map.NotNullable(true);
                map.Type(TypeFactory.Basic(typeof(double).FullName));
            });
            Property(x => x.RA, map =>
            {
                map.Column("RA");
                map.NotNullable(true);
                map.Type(TypeFactory.Basic(typeof(int).FullName));
            });
            Property(x => x.RouteId, map =>
            {
                map.Column("ROUTEID");
                map.Length(40);
                map.Type(TypeFactory.Basic(typeof(string).FullName));
            });
            Property(x => x.SM, map =>
            {
                map.Column("SM");
                map.Length(20);
                map.Type(TypeFactory.Basic(typeof(string).FullName));
            });

            Property(x => x.SN, map =>
            {
                map.Column("SN");
                map.Length(20);
                map.Type(TypeFactory.Basic(typeof(string).FullName));
            });
            Property(x => x.SOR, map =>
            {
                map.Column("SOR");
                map.Length(500);
                map.Type(TypeFactory.Basic(typeof(string).FullName));
            });
            Property(x => x.ST, map =>
            {
                map.Column("ST");
                map.Length(20);
                map.Type(TypeFactory.Basic(typeof(string).FullName));
            });
            Property(x => x.TN, map =>
            {
                map.Column("TN");
                map.Type(TypeFactory.Basic(typeof(double).FullName));
            });
            Property(x => x.TS, map =>
            {
                map.Column("TS");
                map.Length(20);
                map.Type(TypeFactory.Basic(typeof(string).FullName));
            });
            Property(x => x.WG, map =>
            {
                map.Column("WG");
                map.Type(TypeFactory.Basic(typeof(double).FullName));
            });

        }
    }
}
