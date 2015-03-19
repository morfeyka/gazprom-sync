using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sofia.Domain.Setting.Appointment.Sheduling
{
   
    public class EitpDailyScheduling : Sheduler
    {
         public EitpDailyScheduling():this(false)
        {
            
        }

         public EitpDailyScheduling(bool isPeriodicity)
             : base("eitp_daily", isPeriodicity)
        {
        }
    }
}
