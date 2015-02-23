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
    public class DimensionsMap:ClassMapping<Dimensions>
    {
        public DimensionsMap()
        {
            Mutable(false);
            Table("INVENTDIM");
            Where("DATAAREAID='sof'");
            Id(x => x.Id, map =>
                              {
                                  map.Column("INVENTDIMID");
                                  map.Generator(Generators.Assigned);
                                  map.Length(20);
                                  map.Type((IIdentifierType) TypeFactory.Basic(typeof (string).FullName));
                              });
            Property(x=>x.DataAreaId,map=>
                                         {
                                             map.Column("DATAAREAID");
                                             map.Length(3);
                                             map.NotNullable(true);
                                         });
            Property(x=>x.SF_ColorId,map=>
                                         {
                                             map.Column("SF_COLORID");
                                             map.Length(20);
                                             map.NotNullable(true);
                                         });
            Property(x => x.SF_HeightId, map =>
            {
                map.Column("SF_HEIGHTID");
                map.Length(10);
                map.NotNullable(true);
            });
            Property(x => x.SF_WidthId, map =>
            {
                map.Column("SF_WIDTHID");
                map.Length(10);
                map.NotNullable(true);
            });
        }
    }
}
