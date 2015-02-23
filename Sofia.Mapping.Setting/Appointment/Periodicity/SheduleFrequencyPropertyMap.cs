using System;
using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Type;
using Sofia.Domain.Setting.Appointment.Periodicity;

namespace Sofia.Mapping.Setting.Appointment.Periodicity
{
    public class SheduleFrequencyPropertyMap : ClassMapping<SheduleFrequencyProperty>
    {
        public SheduleFrequencyPropertyMap()
        {
            Table("SheduleFrequency");
            Lazy(false);
            Id(x => x.Id, map =>
                              {
                                  map.Column("Id");
                                  map.Generator(Generators.Identity);
                                  map.Type((IIdentifierType) TypeFactory.Basic(typeof (int).FullName));

                                  map.UnsavedValue(0);
                              });
            Discriminator(dm =>
                              {
                                  dm.Column("FrequencyType");
                                  dm.Length(50);
                                  dm.Type(TypeFactory.Basic(typeof (string).FullName));
                                  dm.NotNullable(true);
                                  // dm.Insert(false);
                              });
            Property(x => x.DurationFrom, map =>
                                              {
                                                  map.Access(Accessor.Field);
                                                  map.Column(col =>
                                                                 {
                                                                     col.Name("DurationDateStart");
                                                                     col.SqlType("date");
                                                                 });
                                                  map.NotNullable(true);
                                                  map.Type(TypeFactory.Basic(typeof (DateTime).FullName));
                                              });
            Property(x => x.DurationTo, map =>
                                            {
                                                map.Access(Accessor.Field);
                                                map.Column(col =>
                                                               {
                                                                   col.Name("DurationDateStop");
                                                                   col.SqlType("date");
                                                               });
                                                map.NotNullable(false);
                                                map.Type(TypeFactory.Basic(typeof (DateTime).FullName));
                                            });
            Property(x => x.Period, map =>
                                        {
                                            map.Access(Accessor.Field);
                                            map.Column("RecursEvery");
                                            map.NotNullable(false);
                                            map.Type(TypeFactory.Basic(typeof (int).FullName));
                                        });
            Component(x => x.DailyFrequency, map =>
                                                 {
                                                     map.Access(Accessor.Field);
                                                     map.Class<TimeSpanFrequency>();
                                                     map.Lazy(false);
                                                     map.Property(p => p.Period, m =>
                                                                                     {
                                                                                         map.Access(Accessor.Field);
                                                                                         m.Column(
                                                                                             "EveryDailyOccursValue");
                                                                                         m.NotNullable(true);
                                                                                         m.Type(
                                                                                             TypeFactory.Basic(
                                                                                                 typeof (int).FullName));
                                                                                     });
                                                     map.Property(p => p.OccursEvery, m =>
                                                                                          {
                                                                                              m.Column(
                                                                                                  "EveryDailyOccursValueType");
                                                                                              m.NotNullable(true);
                                                                                              m.Type(
                                                                                                  NHibernateUtil.
                                                                                                      GuessType(
                                                                                                          typeof (
                                                                                                              RhythmByTime
                                                                                                              )));
                                                                                          });
                                                     map.Property(p => p.StartingAt, m =>
                                                                                         {
                                                                                             m.Column(
                                                                                                 "EveryDailyStartingAt");
                                                                                             m.NotNullable(true);
                                                                                             m.Type(
                                                                                                 TypeFactory.Basic(
                                                                                                     typeof (TimeSpan).
                                                                                                         FullName));
                                                                                         });
                                                     map.Property(p => p.EndingAt, m =>
                                                                                       {
                                                                                           m.Column("EveryDailyEndingAt");
                                                                                           m.NotNullable(true);
                                                                                           m.Type(
                                                                                               TypeFactory.Basic(
                                                                                                   typeof (TimeSpan).
                                                                                                       FullName));
                                                                                       });
                                                 });
        }
    }
}