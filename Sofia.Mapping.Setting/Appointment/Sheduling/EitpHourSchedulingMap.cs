using NHibernate.Mapping.ByCode.Conformist;
using Sofia.Domain.Setting.Appointment.Sheduling;

namespace Sofia.Mapping.Setting.Appointment.Sheduling
{
    public class EitpHourSchedulingMap : SubclassMapping<EitpHourScheduling>
    {
        public EitpHourSchedulingMap()
        {
            Lazy(false);
            DiscriminatorValue("eitp_hour");
        }
    }
}