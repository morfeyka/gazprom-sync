using NHibernate.Mapping.ByCode.Conformist;
using Sofia.Domain.Setting.Appointment.Periodicity;

namespace Sofia.Mapping.Setting.Appointment.Periodicity
{
    public class SingleLanchFrequencyMap : SubclassMapping<SingleLanchFrequency>
    {
        public SingleLanchFrequencyMap()
        {
            Lazy(false);
            DiscriminatorValue("single");
        }
    }
}