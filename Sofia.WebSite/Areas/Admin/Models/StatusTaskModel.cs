using System;
using Sofia.Domain.Setting.Log;

namespace Sofia.WebSite.Areas.Admin.Models
{
    public class StatusTaskModel
    {
        public int? TotalRows { get; set; }
        public int Id { get; set; }
        public TaskExecType TaskExecType { get; set; }
        public DateTime StartRun { get; set; }
        public DateTime? EndRun { get; set; }
        public TimeSpan Duration { get; set; }
        public string Error { get; set; }
        public int ShedulerId { get; set; }
        public string ShedulerName { get; set; }

        public int? ErrorRows { get; set; }
    }
}