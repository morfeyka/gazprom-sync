using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping.ByCode.Conformist;
using Sofia.Domain.Setting.Appointment.Sheduling;

namespace Sofia.Mapping.Setting.Appointment.Sheduling
{
    public class EitpDailySchedulerMap : SubclassMapping<EitpDailyScheduling>
    {
        public EitpDailySchedulerMap()
        {
            Lazy(false);
            DiscriminatorValue("eitp_daily");
        }
    }
}
