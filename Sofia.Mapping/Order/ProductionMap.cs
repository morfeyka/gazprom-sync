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
    public class ProductionMap: ClassMapping<Production>
    {
        public ProductionMap()
        {
            Mutable(false);
            Table("PRODTABLE");
            Id(x=>x.Id, map=>
                            {
                                map.Column("PRODID"); 
                                map.Length(40);
                                map.Generator(Generators.Assigned);
                                map.Type((IIdentifierType)TypeFactory.Basic(typeof(string).FullName));
                            });
            Property(x => x.RecId, map =>
            {
                map.Column("RECID");
                map.NotNullable(true);
            });
            Set(p => p.Routes, cm =>
            {
                cm.Access(Accessor.Field);
                cm.Cascade(Cascade.None);
                cm.Fetch(CollectionFetchMode.Select);
                cm.Inverse(true);
                cm.Key(km =>
                {
                    km.Column("ROUTEID");
                    km.NotNullable(false);
                });
                cm.Lazy(CollectionLazy.Lazy);
            }, m => m.OneToMany(om => om.Class(typeof(Route))));
            Set(p => p.ProductionRoutes, cm =>
            {
                cm.Access(Accessor.Field);
                cm.Cascade(Cascade.None);
                cm.Fetch(CollectionFetchMode.Select);
                cm.Inverse(true);
                cm.Key(km =>
                {
                    km.Column("PRODID");
                    km.NotNullable(false);
                });
                cm.Lazy(CollectionLazy.Lazy);
            }, m => m.OneToMany(om => om.Class(typeof(ProductionRoute))));
            Set(p => p.Boms, cm =>
            {
                cm.Access(Accessor.Field);
                cm.Cascade(Cascade.None);
                cm.Fetch(CollectionFetchMode.Select);
                cm.Inverse(true);
                cm.Key(km =>
                {
                    km.Column("PRODID");
                    km.NotNullable(false);
                });
                cm.Lazy(CollectionLazy.Lazy);
            }, m => m.OneToMany(om => om.Class(typeof(ProductionBOM))));
            ManyToOne(x => x.Item, map =>
            {
                map.Cascade(Cascade.None);
                map.Class(typeof(Item));
                map.Column("ITEMID");
                map.Lazy(LazyRelation.Proxy);
                map.NotNullable(false);
                map.NotFound(NotFoundMode.Ignore);
            });
            ManyToOne(x => x.Sale, map =>
            {
                map.Cascade(Cascade.None);
                map.Class(typeof(Sale));
                map.Column("INVENTREFID");
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
            Property(p => p.ProductionStatus, m =>
            {
                m.Access(Accessor.Property);
                m.Column("fiaxProdStatus");
                m.Type(NHibernate.NHibernateUtil.GuessType(typeof(StatusImport)));
            });
            Property(x => x.Name, map => { map.Column("NAME"); map.Length(140); map.NotNullable(false); });
            Property(x => x.GroupId, map => { map.Column("PRODGROUPID"); map.Length(10); map.NotNullable(false); });
            //Property(x => x.ProductionStatus, map => { map.Column("PRODSTATUS"); map.NotNullable(false); });
            Property(x => x.Priority, map => { map.Column("PRODPRIO"); map.NotNullable(false); });
            Property(x => x.DataAreaId, map => { map.Column("DATAAREAID"); map.NotNullable(false); });
            Property(x => x.RouteId, map => { map.Column("ROUTEID"); map.Length(40); map.NotNullable(false); });
            //ManyToOne(x => x.DataImported, map =>
            //                                   {
            //                                       map.PropertyRef("RecId");
            //                                       map.Cascade(Cascade.None);
            //                                       map.Class(typeof(Domain.Interface.DataImported));
            //                                       map.Column("RECID");
            //                                       map.NotNullable(false);
            //                                   });
            Property(x => x.CreatedBy, map => { map.Column("CreatedBy"); map.Length(10); map.NotNullable(false); });
            Property(x => x.DeliveryDate, map => { map.Column("DLVDATE"); map.NotNullable(false); });
            Property(x => x.CollectRefProdId, map => { map.Column("COLLECTREFPRODID"); map.Length(20); map.NotNullable(false); });
            Property(x => x.InventRefType, map => { map.Column("INVENTREFTYPE"); map.NotNullable(false); });
            Property(x => x.QuantityStUp, map => { map.Column("QTYSTUP"); map.NotNullable(false); });
            Property(x => x.SF_ProdTaskNum, map => { map.Column("SF_PRODTASKNUM"); map.Length(10); map.NotNullable(false); });
            Property(x => x.SF_InventLocationIssueId, map => { map.Column("SF_INVENTLOCATIONISSUEID"); map.Length(10); map.NotNullable(false); });
            Property(x => x.SF_NOSTANDART, map => { map.Column("SF_NOSTANDART"); map.NotNullable(false); });
            Property(x => x.QtyCalc, map => { map.Column("QTYCALC"); map.NotNullable(false); });
            Property(x => x.sf_RFIDCommentsID_1, map => { map.Column("SF_RFIDCOMMENTSID_1"); map.Length(20); map.NotNullable(false); });
            Property(x => x.sf_RFIDCommentsID_2, map => { map.Column("SF_RFIDCOMMENTSID_2"); map.Length(20); map.NotNullable(false); });
            Property(x => x.sf_RFIDCommentsID_3, map => { map.Column("SF_RFIDCOMMENTSID_3"); map.Length(20); map.NotNullable(false); });
            Property(x => x.sf_RFIDCommentsID_4, map => { map.Column("SF_RFIDCOMMENTSID_4"); map.Length(20); map.NotNullable(false); });
            Property(x => x.SF_OBLTYPE, map => { map.Column("SF_OBLTYPE"); map.Length(10); map.NotNullable(false); });
            Property(x => x.SF_LAKTYPE_1, map => { map.Column("SF_LAKTYPE_1"); map.NotNullable(false); });
            Property(x => x.SF_LAKTYPE_2, map => { map.Column("SF_LAKTYPE_2"); map.NotNullable(false); });
            Property(x => x.SF_LogisticColors, map => { map.Column("SF_LOGISTICCOLORS"); map.NotNullable(false); });
            Property(x => x.BomId, map => { map.Column("BOMID"); map.Length(40); map.NotNullable(false); });
                        
        }

    }
}
