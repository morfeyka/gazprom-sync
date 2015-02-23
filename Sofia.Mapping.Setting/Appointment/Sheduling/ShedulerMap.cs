using System;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Type;
using Sofia.Domain.Setting.Appointment.Periodicity;
using Sofia.Domain.Setting.Appointment.Sheduling;

namespace Sofia.Mapping.Setting.Appointment.Sheduling
{
    public class ShedulerMap : ClassMapping<Sheduler>
    {
        public ShedulerMap()
        {
            Table("Sheduler");
            Id(x => x.Id, map =>
                              {
                                  map.Column("ID");
                                  map.Generator(Generators.Identity);
                                  map.Type((IIdentifierType) TypeFactory.Basic(typeof (int).FullName));
                                  map.UnsavedValue(0);
                              });
            Discriminator(dm =>
                              {
                                  dm.Column("Type");
                                  dm.Length(50);
                                  dm.Type(TypeFactory.Basic(typeof (string).FullName));
                                  dm.NotNullable(true);
                                  dm.Insert(false);
                              });

            Property(x => x.Type, map =>
            {
                map.Access(Accessor.NoSetter);
                map.Column("Type");
                map.Length(50);
                map.Type(TypeFactory.Basic(typeof(string).FullName));
                map.NotNullable(true);
            });
            Property(x => x.Name, map =>
                                      {
                                          map.Access(Accessor.Property);
                                          map.NotNullable(true);
                                          map.Type(TypeFactory.Basic(typeof (string).FullName));
                                      });
            Property(x => x.Param, map =>
            {
                map.Access(Accessor.Property);
                map.NotNullable(false);
                map.Type(TypeFactory.Basic(typeof(string).FullName));
            });
            Property(x => x.TimeGetData, map =>
            {
                map.Access(Accessor.Property);
                map.NotNullable(false);
                map.Type(TypeFactory.Basic(typeof(DateTime?).FullName));
            });
            Property(x => x.CreatedOn, map =>
                                           {
                                               map.Access(Accessor.NoSetter);
                                               map.Column(x=>x.Default("getdate()"));
                                               map.NotNullable(true);
                                               map.Type(TypeFactory.Basic(typeof (DateTime).FullName));
                                               map.Generated(PropertyGeneration.Insert);
                                               map.Update(false);
                                           });
            Property(x => x.LastRun, map =>
                                         {
                                             map.NotNullable(false);
                                             map.Type(TypeFactory.Basic(typeof (DateTime?).FullName));
                                         });
            Property(x => x.NextRun, map =>
                                         {
                                             map.NotNullable(false);
                                             map.Type(TypeFactory.Basic(typeof (DateTime?).FullName));
                                         });
            Property(x => x.Duration, map =>
                                          {
                                              map.NotNullable(false);
                                              map.Type(TypeFactory.Basic(typeof (decimal?).FullName));
                                          });
            Property(x => x.Runtime, map =>
            {
                map.NotNullable(false);
                map.Type(TypeFactory.Basic(typeof(int?).FullName));
            });
            Property(x => x.Iteration, map =>
                                           {
                                               map.Access(Accessor.Field);
                                               map.NotNullable(true);
                                               map.Type(TypeFactory.Basic(typeof (int).FullName));
                                           });
            Property(x => x.IsEnabled, map =>
                                           {
                                               map.Access(Accessor.Field);
                                               map.NotNullable(true);
                                               map.Type(TypeFactory.Basic(typeof (bool).FullName));
                                           });
            Property(x => x.IsKill, map =>
            {
                map.Access(Accessor.Field);
                map.NotNullable(true);
                map.Column(x => x.Default("0"));
                map.Type(TypeFactory.Basic(typeof(bool).FullName));
            });
            ManyToOne(x => x.Frequency, map =>
                                            {
                                                map.Class(typeof (SheduleFrequencyProperty));
                                                map.Cascade(Cascade.DeleteOrphans | Cascade.All);
                                                map.NotNullable(false);
                                                map.Lazy(LazyRelation.NoProxy);
                                                map.ForeignKey("FK_Sheduler_SheduleFrequency");
                                                map.Access(Accessor.Field);
                                            });
        }
    }
}