using System;
using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Type;
using Sofia.Domain.Setting.Appointment.Periodicity;

namespace Sofia.Mapping.Setting.Appointment.Periodicity
{
    public class MonthlyFrequencyMap : SubclassMapping<MonthlyFrequency>
    {
        public MonthlyFrequencyMap()
        {
            Lazy(false);
            DiscriminatorValue("monthly");
            Join("Frequency", join =>
                                  {
                                      join.Table("MonthlyFrequency");
                                      join.Key(x =>
                                                   {
                                                       x.Column("Frequency");
                                                       x.NotNullable(true);
                                                       x.Unique(true);
                                                       x.ForeignKey("FK_MontlyFrequency_SheduleFrequency");
                                                   });
                                      join.Property(x => x.DayOffset, map =>
                                                                          {
                                                                              map.Column("DayNumber");
                                                                              map.NotNullable(false);
                                                                              map.Access(Accessor.Field);
                                                                              map.Type(
                                                                                  TypeFactory.Basic(
                                                                                      typeof (int?).FullName));
                                                                          });
                                      join.Property(x => x.WeekNumber, map =>
                                                                           {
                                                                               map.NotNullable(false);
                                                                               map.Type(
                                                                                   NHibernateUtil.GuessType(
                                                                                       typeof (RhythmByWeek?)));
                                                                           });
                                      join.Property(x => x.Week, map =>
                                                                     {
                                                                         map.NotNullable(false);
                                                                         map.Type(
                                                                             NHibernateUtil.GuessType(
                                                                                 typeof (DayOfWeek?)));
                                                                     });
                                  });
        }
    }
}