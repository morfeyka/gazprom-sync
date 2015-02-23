using NHibernate.Mapping.ByCode.Conformist;
using Sofia.Domain.Setting.Appointment.Sheduling;

namespace Sofia.Mapping.Setting.Appointment.Sheduling
{
    public class ExportShedulingMap : SubclassMapping<ExportSheduling>
    {
        public ExportShedulingMap()
        {
            Lazy(false);
            DiscriminatorValue("export");
        }
    }
}