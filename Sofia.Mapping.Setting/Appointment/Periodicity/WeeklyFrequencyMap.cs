using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Type;
using Sofia.Domain.Setting.Appointment.Periodicity;

namespace Sofia.Mapping.Setting.Appointment.Periodicity
{
    public class WeeklyFrequencyMap : SubclassMapping<WeeklyFrequency>
    {
        public WeeklyFrequencyMap()
        {
            Lazy(false);
            DiscriminatorValue("weekly");
            Join("Frequency", join =>
                                  {
                                      join.Table("WeeklyFrequency");
                                      join.Fetch(FetchKind.Join);
                                      join.Key(x =>
                                                   {
                                                       x.Column("Frequency");
                                                       x.NotNullable(true);
                                                       x.Unique(true);
                                                       x.ForeignKey("FK_WeeklyFrequency_SheduleFrequency");
                                                   });
                                      join.Property("IsOccursOnMon", map =>
                                                                         {
                                                                             map.Column("OnlyMon");
                                                                             map.NotNullable(true);
                                                                             map.Type(
                                                                                 TypeFactory.Basic(
                                                                                     typeof (bool).FullName));
                                                                         });
                                      join.Property("IsOccursOnTue", map =>
                                                                         {
                                                                             map.Column("OnlyTue");
                                                                             map.NotNullable(true);
                                                                             map.Type(
                                                                                 TypeFactory.Basic(
                                                                                     typeof (bool).FullName));
                                                                         });
                                      join.Property("IsOccursOnWed", map =>
                                                                         {
                                                                             map.Column("OnlyWed");
                                                                             map.NotNullable(true);
                                                                             map.Type(
                                                                                 TypeFactory.Basic(
                                                                                     typeof (bool).FullName));
                                                                         });
                                      join.Property("IsOccursOnThu", map =>
                                                                         {
                                                                             map.Column("OnlyThu");
                                                                             map.NotNullable(true);
                                                                             map.Type(
                                                                                 TypeFactory.Basic(
                                                                                     typeof (bool).FullName));
                                                                         });
                                      join.Property("IsOccursOnFri", map =>
                                                                         {
                                                                             map.Column("OnlyFri");
                                                                             map.NotNullable(true);
                                                                             map.Type(
                                                                                 TypeFactory.Basic(
                                                                                     typeof (bool).FullName));
                                                                         });
                                      join.Property("IsOccursOnSat", map =>
                                                                         {
                                                                             map.Column("OnlySat");
                                                                             map.NotNullable(true);
                                                                             map.Type(
                                                                                 TypeFactory.Basic(
                                                                                     typeof (bool).FullName));
                                                                         });
                                      join.Property("IsOccursOnSun", map =>
                                                                         {
                                                                             map.Column("OnlySun");
                                                                             map.NotNullable(true);
                                                                             map.Type(
                                                                                 TypeFactory.Basic(
                                                                                     typeof (bool).FullName));
                                                                         });
                                  });
        }
    }
}