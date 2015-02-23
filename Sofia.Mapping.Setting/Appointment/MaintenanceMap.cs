using System;
using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Type;
using Sofia.Domain.Setting.Appointment;

namespace Sofia.Mapping.Setting.Appointment
{
    public class MaintenanceMap : ClassMapping<Maintenance>
    {
        public MaintenanceMap()
        {
            Table("Maintenance");
            Id(x => x.Id, map =>
                              {
                                  map.Column("ID");
                                  map.Generator(Generators.Identity);
                                  map.Type((IIdentifierType) TypeFactory.Basic(typeof (int).FullName));
                                  map.UnsavedValue(0);
                              });
            Discriminator(dm =>
                              {
                                  dm.Column("Entity");
                                  dm.Length(50);
                                  dm.Type(TypeFactory.Basic(typeof (string).FullName));
                                  dm.NotNullable(true);
                                  //dm.Insert(false);
                              });
            Property(x => x.CreatedOn, map =>
                                           {
                                               map.Access(Accessor.Field);
                                               map.Column(x => x.Default("getdate()"));
                                               map.Column("CreatedOn");
                                               map.NotNullable(true);
                                               map.Type(TypeFactory.Basic(typeof (DateTime).FullName));
                                           });
            Property(x => x.LaunchedOn, map =>
                                            {
                                                map.Access(Accessor.Property);
                                                map.Column("LaunchedOn");
                                                map.NotNullable(false);
                                                map.Type(TypeFactory.Basic(typeof (DateTime?).FullName));
                                            });
            Property(x => x.CompletedOn, map =>
                                             {
                                                 map.Access(Accessor.Field);
                                                 map.Column("CompletedOn");
                                                 map.NotNullable(false);
                                                 map.Type(TypeFactory.Basic(typeof (DateTime?).FullName));
                                             });
            Property(x => x.JobState, map =>
                                          {
                                              map.Access(Accessor.Field);
                                              map.Column("JobState");
                                              map.NotNullable(true);
                                              map.Type(NHibernateUtil.GuessType(typeof (MaintenanceState)));
                                          });
            Property(x => x.ErrorDescription, map =>
                                                  {
                                                      map.Access(Accessor.Field);
                                                      map.Column("ErrorDescription");
                                                      map.NotNullable(true);
                                                      map.Type(TypeFactory.Basic(typeof (string).FullName));
                                                  });
            Property(x => x.Attempts, map =>
                                          {
                                              map.Access(Accessor.Field);
                                              map.Column("Attempts");
                                              map.NotNullable(true);
                                              map.Type(TypeFactory.Basic(typeof (int).FullName));
                                          });
        }
    }
}