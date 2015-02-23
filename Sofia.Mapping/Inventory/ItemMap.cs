using System;
using HG.Base.NHibernate.Domain;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Type;
using Sofia.Domain.Interface;
using Sofia.Domain.Inventory;

namespace Sofia.Mapping.Inventory
{
    /// <summary>
    /// Представляет связку таблицы InventTable с доменной сущностью <see cref="Item"/>.
    /// </summary>
    public  class ItemMap : ClassMapping<Item>
    {
        public ItemMap()
        {
            Mutable(false);
            Table("INVENTTABLE");
            Lazy(false);
            Id(x => x.Id, map =>
                              {
                                  map.Column("ITEMID");
                                  map.Generator(Generators.Assigned);
                                  map.Type((IIdentifierType)TypeFactory.Basic(typeof(string).FullName));
                              });


            Set(p => p.BarcodeItems, cm =>
            {
                cm.Access(Accessor.Field);
                cm.Cascade(Cascade.None);
                cm.Fetch(CollectionFetchMode.Select);
                cm.Inverse(true);
                cm.Key(km =>
                {
                    km.Column("ITEMID");
                    km.NotNullable(false);
                });
                cm.Lazy(CollectionLazy.Lazy);
            }, m => m.OneToMany(om => om.Class(typeof(BarcodeItem))));
            ManyToOne(x => x.Group, map =>
            {
                map.Cascade(Cascade.None);
                map.Class(typeof(Group));
                map.Column("ITEMGROUPID");
                map.Lazy(LazyRelation.Proxy);
                map.NotNullable(false);
                map.NotFound(NotFoundMode.Ignore);
            });

            Set(p => p.UnitConverts, cm =>
            {
                cm.Access(Accessor.Field);
                cm.Cascade(Cascade.None);
                cm.Fetch(CollectionFetchMode.Select);
                cm.Inverse(true);
                cm.Key(km =>
                {
                    km.Column("ITEMID");
                    km.NotNullable(false);
                });
                cm.Lazy(CollectionLazy.Lazy);
            }, m => m.OneToMany(om => om.Class(typeof(UnitConvert))));
            Set(p => p.PriceItems, cm =>
            {
                cm.Access(Accessor.Field);
                cm.Cascade(Cascade.None);
                cm.Fetch(CollectionFetchMode.Select);
                cm.Inverse(true);
                cm.Key(km =>
                {
                    km.Column("ITEMID");
                    km.NotNullable(false);
                });
                cm.Lazy(CollectionLazy.Lazy);
            }, m => m.OneToMany(om => om.Class(typeof(PriceItem))));
            Property(x => x.RecId, map =>
            {
                map.Column("RECID");
                map.NotNullable(true);
            });
            Property(x => x.Name, map =>
                                      {
                                          map.Column("ITEMNAME");
                                          map.Length(140);
                                          map.NotNullable(false);
                                      });
            Property(x => x.SF_PBAMODELTYPEID, map =>
                                                   {
                                                       map.Column("SF_PBAMODELTYPEID");
                                                       map.Length(20);
                                                       map.NotNullable(false);
                                                   });
            ManyToOne(x => x.SizeType, map =>
                                           {
                                               map.Cascade(Cascade.None);
                                               map.Class(typeof (SizeType));
                                               map.Column("SF_SIZETYPEID");
                                               map.Lazy(LazyRelation.Proxy);
                                               map.NotNullable(true);
                                               map.NotFound(NotFoundMode.Ignore);
                                           });
            ManyToOne(x => x.Container, map =>
            {
                map.Cascade(Cascade.None);
                map.Class(typeof(ItemContainer));
                map.Column("RECID");
                map.Lazy(LazyRelation.Proxy);
                map.NotNullable(false);
            });

            Property(x => x.SF_Height, map =>
                                           {
                                               map.Column("SF_HEIGHT"); 
                                               map.NotNullable(false);
                                           });
            Property(x => x.SF_Width, map =>
                                          {
                                              map.Column("SF_WIDTH");
                                              map.NotNullable(false);
                                          });
            Property(x => x.GrossHeight, map =>
                                             {
                                                 map.Column("GROSSHEIGHT");
                                                 map.NotNullable(false);
                                             });
            Property(x => x.GrossDepth, map =>
                                            {
                                                map.Column("GROSSDEPTH");
                                                map.NotNullable(false);
                                            });
            Property(x => x.GrossWidth, map =>
                                            {
                                                map.Column("GROSSWIDTH");
                                                map.NotNullable(false);
                                            });
            Property(x => x.SF_ColorOfMaterialId_1, map =>
                                                        {
                                                            map.Column("SF_COLOROFMATERIALID_1");
                                                            map.Length(20);
                                                            map.NotNullable(false);
                                                        });
            ManyToOne(x => x.BomUnit, map =>
                                          {
                                              map.Cascade(Cascade.None);
                                              map.Class(typeof(Unit));
                                              map.Column("BOMUNITID");
                                              map.Lazy(LazyRelation.Proxy);
                                              map.NotNullable(false);
                                          });
        }
    }
}
