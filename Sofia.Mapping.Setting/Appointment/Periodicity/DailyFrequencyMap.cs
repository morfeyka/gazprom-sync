using NHibernate.Mapping.ByCode.Conformist;
using Sofia.Domain.Setting.Appointment.Periodicity;

namespace Sofia.Mapping.Setting.Appointment.Periodicity
{
    public class DailyFrequencyMap : SubclassMapping<DailyFrequency>
    {
        public DailyFrequencyMap()
        {
            Lazy(false);
            DiscriminatorValue("daily");
        }
    }
}