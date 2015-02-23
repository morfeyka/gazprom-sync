using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Type;
using Sofia.Domain.Interface;

namespace Sofia.Mapping.Interface
{
    public class DataImportedMap : ClassMapping<DataImported>
    {
        public DataImportedMap()
        {
            Mutable(false);
			Table("FIAXINTFTABLE");
            Where("REFTABLEID in (175, 152, 262)");
            Id(x => x.Id, map =>
                              {
                                  map.Column("RECID");
                                  map.Generator(Generators.Assigned);
                                  map.Type((IIdentifierType) TypeFactory.Basic(typeof (long).FullName));
                              });
            Discriminator(dm =>
                              {
                                  dm.Column("REFTABLEID");
                                  dm.NotNullable(true);
                                  dm.Insert(false);
                              });
            Property(p => p.TypeData, m =>
            {
                m.Access(Accessor.Field);
                m.Column("REFTABLEID");
                m.Type(NHibernate.NHibernateUtil.GuessType(typeof(TypeData)));
            });
			//Property(x => x.REFID, map => { map.Column("REFID"); map.Length(40); map.NotNullable(false); });
			//Property(x => x.REFID1, map => { map.Column("REFID1"); map.Length(40); map.NotNullable(false); });
			Property(x => x.Action, map =>
			                            {
			                                map.Column("ACTION"); 
                                            map.NotNullable(false);
                                            map.Type(NHibernate.NHibernateUtil.GuessType(typeof(Domain.Interface.Action)));
			                            });
			Property(x => x.Status, map =>
			                            {
			                                map.Column("STATUS"); 
                                            map.NotNullable(false);
                                            map.Type(NHibernate.NHibernateUtil.GuessType(typeof(Domain.Interface.Status)));
			                            });
			Property(x => x.Message, map =>
			                             {
			                                 map.Column("MESSAGE"); 
                                             map.Length(254); 
                                             map.NotNullable(false);
			                             });
			//Property(x => x.REFID2, map => { map.Column("REFID2"); map.Length(40); map.NotNullable(false); });
			Property(x => x.RecId, map =>
			                           {
			                               map.Column("REFRECID");
                                           map.NotNullable(false);
			                           });
			Property(x => x.ModifiedDate, map => { map.Column("MODIFIEDDATE"); map.NotNullable(false); });
			Property(x => x.ModifiedTime, map => { map.Column("MODIFIEDTIME"); map.NotNullable(false); });
			//Property(x => x.CREATEDDATE, map => { map.Column("CREATEDDATE"); map.NotNullable(false); });
			//Property(x => x.CREATEDTIME, map => { map.Column("CREATEDTIME"); map.NotNullable(false); });
			Property(x => x.CreatedBy, map =>
			                               {
			                                   map.Column("CREATEDBY"); 
                                               map.Length(5); 
                                               map.NotNullable(false);
			                               });
			Property(x => x.DataAreaId, map =>
			                                {
			                                    map.Column("DATAAREAID"); 
                                                map.Length(3); 
                                                map.NotNullable(false);
			                                });
			//Property(x => x.RECVERSION, map => { map.Column("RECVERSION"); map.NotNullable(false); });
        }
    }
}
