using NHibernate.Mapping.ByCode.Conformist;
using Sofia.Domain.Setting.Appointment.Sheduling;

namespace Sofia.Mapping.Setting.Appointment.Sheduling
{
    public class ImportShedulingMap : SubclassMapping<ImportSheduling>
    {
        public ImportShedulingMap()
        {
            Lazy(false);
            DiscriminatorValue("import");
        }
    }
}