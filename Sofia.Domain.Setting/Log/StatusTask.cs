using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sofia.Domain.Setting.Log
{
    public class StatusTask : Entity
    {
        public virtual TaskExecType TaskExecType { get; set; }
        public virtual DateTime StartRun { get; set; }
        public virtual DateTime? EndRun { get; set; }
        public virtual string Error { get; set; }
        public virtual int? TotalRows { get; set; }
        public virtual int? ErrorRows { get; set; }
        public virtual Appointment.Sheduling.Sheduler Sheduler { get; set; }
    }
}
