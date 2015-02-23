using NHibernate.Mapping.ByCode.Conformist;
using Sofia.Domain.Setting.Appointment.Sheduling;

namespace Sofia.Mapping.Setting.Appointment.Sheduling
{
    public class MoreShedulingMap : SubclassMapping<MoreSheduling>
    {
        public MoreShedulingMap()
        {
            Lazy(false);
            DiscriminatorValue("more");
        }
    }
}